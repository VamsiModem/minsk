using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Minsk.CodeAnalysis.Text;

namespace Minsk.CodeAnalysis.Syntax
{
     public sealed class SyntaxTree{
        public SyntaxTree(SourceText text, ImmutableArray<Diagnostic> diags, ExpressionSyntax root, SyntaxToken eofToken)
        {
            Text = text;
            Diagnostics = diags;
            Root = root;
            eofToken = EofToken;
        }

        public SourceText Text { get; }
        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken EofToken { get; }
        public static SyntaxTree Parse(string text)
        {
            var sourceText = SourceText.From(text);
            return Parse(sourceText);
        }

        public static SyntaxTree Parse(SourceText text)
        {
            var parser = new Parser(text);
            return parser.Parse();
        }

        public static IEnumerable<SyntaxToken> ParseTokens(SourceText text){
            var lexer = new Lexer(text);
            while(true){
                var token = lexer.Lex();
                if(token.Kind == SyntaxKind.EOFToken) break;
                yield return token;
            }
        }
        public static IEnumerable<SyntaxToken> ParseTokens(string text){
            var sourceText = SourceText.From(text);
            return ParseTokens(sourceText);
        }
    }
    
}