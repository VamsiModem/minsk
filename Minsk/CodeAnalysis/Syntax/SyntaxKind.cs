﻿namespace Minsk.CodeAnalysis.Syntax
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
        UnaryExpression,
        TrueKeyword,
        FalseKeyword,
        Identifier,
        BangToken,
        AmpresandAmpresandToken,
        PipePipeToken,
        EqualsEqualsToken,
        BangEqualsToken
    }
}