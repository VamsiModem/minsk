using System.Collections.Generic;
using System.Linq;

namespace Minsk.Code
{
    class SyntaxToken :SyntaxNode{
        public readonly int position;
        public readonly string text;
        public readonly object value;

        public SyntaxToken(SyntaxKind kind, int position, string text, object value)
        {
            this.Kind = kind;
            this.position = position;
            this.text = text;
            this.value = value;
        }

        public override SyntaxKind Kind{ get;}
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Enumerable.Empty<SyntaxNode>();
        }
    }
}