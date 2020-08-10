namespace Minsk.CodeAnalysis.Binding
{
    internal enum BoundNodeKind{
        UnaryExpression,
        LiteralExpression,
        BinaryExpression,
        VariableExpression,
        AssignmentExpression,
        
        VariableDeclaration,
        IfStatement,
        WhileStatement,
        ForStatement,
        BlockStatement,
        ExpressionStatement,
        GotoStatement,
        LabelStatement,
        ConditionalGotoStatement,
    }
}