using System.Collections.Generic;

namespace Minsk.Code
{
    internal sealed class Parser
    {
        private readonly SyntaxToken[] _tokens;
        private int _position;
        private List<string> _diagnostics = new List<string>();
        public IEnumerable<string> Diagnostics => _diagnostics;
        public Parser(string text)
        {
            var lexer = new Lexer(text);
            var tokens = new List<SyntaxToken>();
            SyntaxToken token;
            do{
                token = lexer.Lex();
                if(token.Kind != SyntaxKind.WhiteSpaceToken && token.Kind != SyntaxKind.BadToken){
                    tokens.Add(token);
                }
            }while(token.Kind != SyntaxKind.EOFToken);
            _tokens = tokens.ToArray();
            _diagnostics.AddRange(lexer.Diagnostics);
        }

        private SyntaxToken Peek(int offset){
            var index = _position + offset;
            if(index >= _tokens.Length)
                return _tokens[_tokens.Length - 1];
            return _tokens[index];
        }

        private SyntaxToken NextToken(){
            var current = Current;
            _position++;
            return current;
        }

        private ExpressionSyntax ParseExpression(int parentPrecedence = 0){
            var left = ParsePrimaryExpression();
            while(true){
                var precedence = GetBinaryOperatorPrecendence(Current.Kind);
                if(precedence == 0 || precedence <= parentPrecedence)
                    break;
                var operatorToken = NextToken();
                var right = ParseExpression(precedence);
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }
            return left;
        }

        private static int GetBinaryOperatorPrecendence(SyntaxKind kind){
            switch(kind){
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 1;
                case SyntaxKind.StarToken:
                case SyntaxKind.SlashToken:
                    return 2;
                default:
                    return 0;
            }
        }
        private SyntaxToken MatchToken(SyntaxKind kind){
            if(Current.Kind == kind)
                return NextToken();
            _diagnostics.Add($"Error: Unexpected token <{Current.Kind}>, expected <{kind}>");
            return new SyntaxToken(kind, Current.position, null, null);
        }
        private SyntaxToken Current => Peek(0);
    
        public SyntaxTree Parse(){
            var expr = ParseExpression();
            var eofToken = MatchToken(SyntaxKind.EOFToken);
            return new SyntaxTree(_diagnostics, expr, eofToken);
        }

        private ExpressionSyntax ParsePrimaryExpression(){
            if(Current.Kind == SyntaxKind.LParenToken){
                var left = NextToken();
                var expr = ParseExpression();
                var right = MatchToken(SyntaxKind.RParenToken);
                return new ParenthesizedExpressionSyntax(left, expr, right);
            }
            var numberToken = MatchToken(SyntaxKind.NumberToken);
            return new LiteralExpressionSyntax(numberToken);
        }
    }
}