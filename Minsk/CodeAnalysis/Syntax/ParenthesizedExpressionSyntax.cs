using System.Collections.Generic;

namespace Minsk.CodeAnalysis.Syntax
{
    public sealed class ParenthesizedExpressionSyntax : ExpressionSyntax
    {
        public ParenthesizedExpressionSyntax(SyntaxToken lParenToken, ExpressionSyntax expression, SyntaxToken rParenToken)
        {
            LParenToken = lParenToken;
            Expression = expression;
            RParenToken = rParenToken;
        }
        public override SyntaxKind Kind => SyntaxKind.ParenthesizedExpression;

        public SyntaxToken LParenToken { get; }
        public ExpressionSyntax Expression { get; }
        public SyntaxToken RParenToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return LParenToken;
            yield return Expression;
            yield return RParenToken;
        }
    }
}