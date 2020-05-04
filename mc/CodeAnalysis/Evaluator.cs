using System;
using Minsk.Code.Binding;
using Minsk.Code.Syntax;
namespace Minsk.Code
{
    internal sealed class Evaluator
    {
        private readonly BoundExpression _root;

        public Evaluator(BoundExpression root)
        {
            _root = root;
        }

        public object Evaluate(){
            return EvaluateExpression(_root);
        }

        private object EvaluateExpression(BoundExpression root)
        {
            if(root is BoundLiteralExpression n)
                return n.Value;
            if(root is BoundUnaryExpression u){
                var operand = EvaluateExpression(u.Operand);
                switch (u.OperatorKind)
                {
                    case BoundUnaryOperatorKind.Identity:
                        return (int)operand;
                    case BoundUnaryOperatorKind.Negation:
                        return -(int)operand;
                    case BoundUnaryOperatorKind.LogicalNegation:
                        return !(bool)operand;
                    default:
                        throw new Exception($"Unexpected binary operator {u.OperatorKind}");
                }
            }
            if(root is BoundBinaryExpression b)
            {
                var left =  EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);
                switch (b.OperatorKind)
                {
                    case BoundBinaryOperatorKind.Addition:
                        return (int)left + (int)right;
                    case BoundBinaryOperatorKind.Subtraction:
                        return (int)left - (int)right;
                    case BoundBinaryOperatorKind.Multiplication:
                        return (int)left * (int)right;
                    case BoundBinaryOperatorKind.Division:
                        return (int)left / (int)right;
                    case BoundBinaryOperatorKind.LogicalAnd:
                        return (bool)left && (bool)right;
                    case BoundBinaryOperatorKind.LogicalOr:
                        return (bool)left || (bool)right;
                    default:
                        throw new Exception($"Unexpected binary operator {b.OperatorKind}");
                }
            }
            else throw new Exception($"Unexpected node {root.Kind}");
        }
    }
}