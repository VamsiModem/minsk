using System.Collections.Generic;
using System.Linq;

namespace Minsk.Code.Syntax
{
    public sealed class SyntaxToken :SyntaxNode{
        public int Position  {get;}
        public string Text {get;}
        public object Value {get;}
        public SyntaxToken(SyntaxKind kind, int position, string text, object value)
        {
            this.Kind = kind;
            Position = position;
            Text = text;
            Value = value;
        }

        public override SyntaxKind Kind{ get;}
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Enumerable.Empty<SyntaxNode>();
        }
    }
}