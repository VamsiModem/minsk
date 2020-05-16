using System.Collections.Generic;
using System.Collections.Immutable;

namespace Minsk.CodeAnalysis.Syntax
{
    internal sealed class Parser
    {
        private readonly ImmutableArray<SyntaxToken> _tokens;
        private int _position;
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
        public DiagnosticBag Diagnostics => _diagnostics;
        private SyntaxToken Current => Peek(0);
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
            _tokens = tokens.ToImmutableArray();
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
            _diagnostics.ReportUnexpectedToken(Current.Span, Current.Kind, kind);
            return new SyntaxToken(kind, Current.Position, null, null);
        }
        public SyntaxTree Parse(){
            var expr = ParseExpression();
            var eofToken = MatchToken(SyntaxKind.EOFToken);
            return new SyntaxTree(_diagnostics.ToImmutableArray(), expr, eofToken);
        }
        private ExpressionSyntax ParseExpression(){
            return ParseAssignmentExpression();
        }
         private ExpressionSyntax ParseAssignmentExpression(){
            if(Peek(0).Kind == SyntaxKind.IdentifierToken && Peek(1).Kind == SyntaxKind.EqualsToken){
                var identifierToken = NextToken();
                var operatorToken = NextToken();
                var right =ParseAssignmentExpression();
                return new AssignmentExpressionSyntax(identifierToken, operatorToken, right);
            }
            return ParseBinaryExpression();
        }
        private ExpressionSyntax ParseBinaryExpression(int parentPrecedence = 0){
            ExpressionSyntax left;
            var unaryOperatorPrecendence = Current.Kind.GetUnaryOperatorPrecedence();
            if(unaryOperatorPrecendence != 0 && unaryOperatorPrecendence >= parentPrecedence){
                var operatorToken = NextToken();
                var operand = ParseBinaryExpression(unaryOperatorPrecendence);
                left = new UnaryExpressionSyntax(operatorToken, operand);
            }else{
                left = ParsePrimaryExpression();
            }
            while(true){
                var precedence = Current.Kind.GetBinaryOperatorPrecedence();
                if(precedence == 0 || precedence <= parentPrecedence)
                    break;
                var operatorToken = NextToken();
                var right = ParseBinaryExpression(precedence);
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }
            return left;
        }
        private ExpressionSyntax ParsePrimaryExpression(){
            switch (Current.Kind)
            {
                case SyntaxKind.LParenToken:
                    return ParseParenthesizedExpression();
                case SyntaxKind.TrueKeyword:
                case SyntaxKind.FalseKeyword:
                    return ParseBooleanLiteral();
                case SyntaxKind.NumberToken:
                    return ParseNumberLiteral();
                case SyntaxKind.NameExpression:
                default:
                    return ParseNameExpression();
            }
        }
        private ExpressionSyntax ParseNumberLiteral()
        {
            var numberToken = MatchToken(SyntaxKind.NumberToken);
            return new LiteralExpressionSyntax(numberToken);
        }
        private ExpressionSyntax ParseParenthesizedExpression()
        {
            var left = MatchToken(SyntaxKind.LParenToken);
            var expr = ParseExpression();
            var right = MatchToken(SyntaxKind.RParenToken);
            return new ParenthesizedExpressionSyntax(left, expr, right);
        }
        private ExpressionSyntax ParseBooleanLiteral()
        {
            var isTrue = Current.Kind == SyntaxKind.TrueKeyword;
            var keywordToken = isTrue ? MatchToken(SyntaxKind.TrueKeyword) : MatchToken(SyntaxKind.FalseKeyword);
            return new LiteralExpressionSyntax(keywordToken, isTrue);
        }
        private ExpressionSyntax ParseNameExpression()
        {
            var identifierToken = MatchToken(SyntaxKind.IdentifierToken);
            return new NameExpressionSyntax(identifierToken);
        }
    }
}