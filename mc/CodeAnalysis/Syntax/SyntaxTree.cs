using System.Collections.Generic;
using System.Linq;

namespace Minsk.Code.Syntax
{
    internal sealed class SyntaxTree{
        public SyntaxTree(IEnumerable<string> diags, ExpressionSyntax root, SyntaxToken eofToken)
        {
            Diagnostics = diags.ToArray();
            Root = root;
            eofToken = EofToken;
        }
        public IReadOnlyList<string> Diagnostics { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken EofToken { get; }
    }
}