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
        IdentifierToken,
        BangToken,
        AmpresandAmpresandToken,
        PipePipeToken,
        EqualsEqualsToken,
        BangEqualsToken,
        NameExpression,
        AssignmentExpression,
        EqualsToken
    }
}