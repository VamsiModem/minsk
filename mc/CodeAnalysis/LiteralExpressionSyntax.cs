using System.Collections.Generic;

namespace Minsk.Code
{
    sealed class LiteralExpressionSyntax : ExpressionSyntax
    {
        public LiteralExpressionSyntax(SyntaxToken literalToken)
        {
            this.LiteralToken = literalToken;
        }
        public override SyntaxKind Kind => SyntaxKind.NumberExpression;

        public SyntaxToken LiteralToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return LiteralToken;
        }
    }
}