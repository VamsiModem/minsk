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
       
        NumberExpression,
        BinaryExpression,
        ParenthesizedExpression
    }
}