namespace Minsk.Code
{
    public enum SyntaxKind{

        BadToken,
        EOFToken,
        WhiteSpaceToken,

        NumberToken, 
        
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        LParenToken,
        RParenToken,
       
        LiteralExpression,
        BinaryExpression,
        ParenthesizedExpression,
        UnaryExpression
    }
}