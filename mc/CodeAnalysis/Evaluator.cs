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

        private int EvaluateExpression(BoundExpression root)
        {
            if(root is BoundLiteralExpression n)
                return (int)n.Value;
            if(root is BoundUnaryExpression u){
                var operand = EvaluateExpression(u.Operand);
                switch (u.OperatorKind)
                {
                    case BoundUnaryOperatorKind.Identity:
                        return operand;
                    case BoundUnaryOperatorKind.Negation:
                        return -operand;
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
                        return left + right;
                    case BoundBinaryOperatorKind.Subtraction:
                        return left - right;
                    case BoundBinaryOperatorKind.Multiplication:
                        return left * right;
                    case BoundBinaryOperatorKind.Division:
                        return left / right;
                    default:
                        throw new Exception($"Unexpected binary operator {b.OperatorKind}");
                }
            }
            else throw new Exception($"Unexpected node {root.Kind}");
        }
    }
}