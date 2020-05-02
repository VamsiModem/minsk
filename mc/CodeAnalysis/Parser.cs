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
                token = lexer.NextToken();
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

        private SyntaxToken MatchToken(SyntaxKind kind){
            if(Current.Kind == kind)
                return NextToken();
            _diagnostics.Add($"Error: Unexpected token <{Current.Kind}>, expected <{kind}>");
            return new SyntaxToken(kind, Current.position, null, null);
        }
        private SyntaxToken Current => Peek(0);
        public ExpressionSyntax ParseTerm(){
            var left = ParseFactor();
            while(Current.Kind == SyntaxKind.PlusToken || Current.Kind == SyntaxKind.MinusToken){
                var operatorToken = NextToken();
                var right = ParseFactor();
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }
            return left;
        }
        public ExpressionSyntax ParseFactor(){
            var left = ParsePrimaryExpression();
            while(Current.Kind == SyntaxKind.StarToken || Current.Kind == SyntaxKind.SlashToken){
                var operatorToken = NextToken();
                var right = ParsePrimaryExpression();
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }
            return left;
        }

        private ExpressionSyntax ParseExpression(){
            return ParseTerm();
        }
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