namespace Minsk.Code
{
    enum SyntaxKind{
        NumberToken, 
        WhiteSpaceToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        LParenToken,
        RParenToken,
        BadToken,
        EOFToken,
        NumberExpression,
        BinaryExpression,
        ParenthesizedExpression
    }
}