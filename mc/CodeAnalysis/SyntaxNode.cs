using System.Collections.Generic;

namespace Minsk.Code
{
    abstract class SyntaxNode{
        public abstract SyntaxKind Kind { get;}
        public abstract IEnumerable<SyntaxNode> GetChildren();
    }
}