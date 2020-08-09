using System;
using System.Collections.Generic;

namespace Minsk.CodeAnalysis.Syntax
{
    public static class SyntaxFacts{
        public static int GetBinaryOperatorPrecedence(this SyntaxKind kind){
            switch(kind){
                
                case SyntaxKind.StarToken:
                case SyntaxKind.SlashToken:
                    return 5;
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 4;
                case SyntaxKind.EqualsEqualsToken:
                case SyntaxKind.BangEqualsToken:
                case SyntaxKind.LessToken:
                case SyntaxKind.LessOrEqualsToken:
                case SyntaxKind.GreaterToken:
                case SyntaxKind.GreaterOrEqualsToken:
                    return 3;
                case SyntaxKind.AmpresandAmpresandToken:
                    return 2;
                case SyntaxKind.PipePipeToken:
                    return 1;
                default:
                    return 0;
            }
        }
        public static int GetUnaryOperatorPrecedence(this SyntaxKind kind){
            switch(kind){
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                case SyntaxKind.BangToken:
                    return 6;
                default:
                    return 0;
            }
        }

        internal static SyntaxKind GetKeyWordKind(string text)
        {
            switch(text){
                case "false":
                    return SyntaxKind.FalseKeyword;
                case "else":
                    return SyntaxKind.ElseKeyword;
                case "if":
                    return SyntaxKind.IfKeyword;
                case "for":
                    return SyntaxKind.ForKeyword;
                case "let":
                    return SyntaxKind.LetKeyword;
                case "var":
                    return SyntaxKind.VarKeyword;
                case "true":
                    return SyntaxKind.TrueKeyword;
                case "while":
                    return SyntaxKind.WhileKeyword;
                case "to":
                    return SyntaxKind.ToKeyword;
                
                default:
                    return SyntaxKind.IdentifierToken;
            }
        }

        public static IEnumerable<SyntaxKind> GetBinaryOperators(){
            var kinds = (SyntaxKind[])Enum.GetValues(typeof(SyntaxKind));
            foreach(var kind in kinds){
                if(GetBinaryOperatorPrecedence(kind) > 0)
                    yield return kind;
            }
        }

        public static IEnumerable<SyntaxKind> GetUnaryOperators(){
            var kinds = (SyntaxKind[])Enum.GetValues(typeof(SyntaxKind));
            foreach(var kind in kinds){
                if(GetUnaryOperatorPrecedence(kind) > 0)
                    yield return kind;
            }
        }

        public static string GetText(SyntaxKind kind){
            switch(kind){
                case SyntaxKind.PlusToken: 
                    return "+";
                case SyntaxKind.MinusToken: 
                    return "-";
                case SyntaxKind.StarToken: 
                    return "*";
                case SyntaxKind.SlashToken: 
                    return "/";
                case SyntaxKind.BangToken: 
                    return "!";
                case SyntaxKind.AmpresandAmpresandToken: 
                    return "&&";
                case SyntaxKind.LParenToken: 
                    return "(";
                case SyntaxKind.RParenToken: 
                    return ")";
                case SyntaxKind.LBraceToken: 
                    return "{";
                case SyntaxKind.RBraceToken: 
                    return "}";
                case SyntaxKind.PipePipeToken: 
                    return "||";
                case SyntaxKind.EqualsEqualsToken: 
                    return "==";
                case SyntaxKind.BangEqualsToken: 
                    return "!=";
                case SyntaxKind.EqualsToken: 
                    return "=" ;
                case SyntaxKind.LessToken: 
                    return "<" ;
                case SyntaxKind.LessOrEqualsToken: 
                    return "<=" ;
                case SyntaxKind.GreaterToken: 
                    return ">" ;
                case SyntaxKind.GreaterOrEqualsToken: 
                    return ">=" ;
                case SyntaxKind.ElseKeyword: 
                    return "else";
                case SyntaxKind.TrueKeyword: 
                    return "true";
                case SyntaxKind.IfKeyword: 
                    return "if";
                case SyntaxKind.FalseKeyword: 
                    return "false";
                case SyntaxKind.LetKeyword: 
                    return "let";
                case SyntaxKind.WhileKeyword: 
                    return "while";
                case SyntaxKind.VarKeyword: 
                    return "var";
                 case SyntaxKind.ForKeyword: 
                    return "for";
                case SyntaxKind.ToKeyword: 
                    return "to";
                default:
                    return null;
            }
        }
    }
}