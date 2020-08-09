namespace Minsk.CodeAnalysis.Syntax
{
    public sealed class ForStatementSyntax : StatementSyntax
    {
        public ForStatementSyntax(SyntaxToken keyword, SyntaxToken identifier, SyntaxToken equalsToken
, ExpressionSyntax lowerBound, ExpressionSyntax upperBound, StatementSyntax body, SyntaxToken toKeyword)
        {
            Keyword = keyword;
            Identifier = identifier;
            EqualsToken = equalsToken;
            LowerBound = lowerBound;
            UpperBound = upperBound;
            Body = body;
            ToKeyword = toKeyword;
        }
        public override SyntaxKind Kind => SyntaxKind.ForStatement;

        public SyntaxToken Keyword { get; }
        public SyntaxToken Identifier { get; }
        public SyntaxToken EqualsToken { get; }
        public ExpressionSyntax LowerBound { get; }
        public ExpressionSyntax UpperBound { get; }
        public StatementSyntax Body { get; }
        public SyntaxToken ToKeyword { get; }
    }
}
        
    
    
