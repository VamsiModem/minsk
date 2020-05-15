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
                case "true":
                    return SyntaxKind.TrueKeyword;
                case "false":
                    return SyntaxKind.FalseKeyword;
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
                case SyntaxKind.PipePipeToken: 
                    return "||";
                case SyntaxKind.EqualsEqualsToken: 
                    return "==";
                case SyntaxKind.BangEqualsToken: 
                    return "!=";
                case SyntaxKind.EqualsToken: 
                    return "=" ;
                case SyntaxKind.TrueKeyword: 
                    return "true";
                case SyntaxKind.FalseKeyword: 
                    return "false";
                default:
                    return null;
            }
        }
    }
}