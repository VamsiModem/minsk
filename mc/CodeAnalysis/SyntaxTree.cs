using System.Collections.Generic;
using System.Linq;

namespace Minsk.Code
{
    internal sealed class SyntaxTree{
        public SyntaxTree(IEnumerable<string> diags, ExpressionSyntax root, SyntaxToken eofToken)
        {
            Diags = diags.ToArray();
            Root = root;
            eofToken = EofToken;
        }

        public IReadOnlyList<string> Diags { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken EofToken { get; }
    }
}