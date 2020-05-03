using System;

namespace Minsk.Code.Binding
{
    internal abstract class BoundExpression: BoundNode{
        public abstract Type Type {get;}
    }
}