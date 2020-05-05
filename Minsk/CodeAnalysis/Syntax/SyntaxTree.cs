using System.Collections.Generic;
using System.Linq;

namespace Minsk.CodeAnalysis.Syntax
{
     public sealed class SyntaxTree{
        public SyntaxTree(IEnumerable<Diagnostic> diags, ExpressionSyntax root, SyntaxToken eofToken)
        {
            Diagnostics = diags.ToArray();
            Root = root;
            eofToken = EofToken;
        }
        public IReadOnlyList<Diagnostic> Diagnostics { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken EofToken { get; }
        public static SyntaxTree Parse(string text)
        {
            var parser = new Parser(text);
            return parser.Parse();
        }
    }
    
}