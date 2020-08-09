namespace Minsk.CodeAnalysis.Syntax
{
    public sealed class IfStatementSyntax : StatementSyntax
    {
        public override SyntaxKind Kind => SyntaxKind.IfStatement;

        public SyntaxToken IfKeyword { get; }
        public ExpressionSyntax Condition { get; }
        public StatementSyntax ThenStatement { get; }
        public ElseClauseSyntax ElseClauseSyntax { get; }

        public IfStatementSyntax(SyntaxToken ifKeyword, ExpressionSyntax contition,
                                StatementSyntax thenStatement, ElseClauseSyntax elseClauseSyntax)
        {
            IfKeyword = ifKeyword;
            Condition = contition;
            ThenStatement = thenStatement;
            ElseClauseSyntax = elseClauseSyntax;
        }

    }
}
        
    
    
