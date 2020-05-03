using System;
using Minsk.Code.Syntax;
namespace Minsk.Code
{
    public class Evaluator
    {
        private readonly ExpressionSyntax _root;

        public Evaluator(ExpressionSyntax root)
        {
            _root = root;
        }

        public int Evaluate(){
            return EvaluateExpression(_root);
        }

        private int EvaluateExpression(ExpressionSyntax root)
        {
            if(root is LiteralExpressionSyntax n)
                return (int)n.LiteralToken.value;
            if(root is UnaryExpressionSyntax u){
                var operand = EvaluateExpression(u.Operand);
                if(u.OperatorToken.Kind == SyntaxKind.PlusToken)
                    return operand;
                else if(u.OperatorToken.Kind == SyntaxKind.MinusToken)
                    return -operand;
                else 
                    throw new Exception($"Unexpected binary operator {u.OperatorToken.Kind}");
            }
            if(root is BinaryExpressionSyntax b)
            {
                var left =  EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);
                if(b.OperatorToken.Kind == SyntaxKind.PlusToken)
                    return left + right;
                else if(b.OperatorToken.Kind == SyntaxKind.MinusToken)
                    return left - right;
                else if(b.OperatorToken.Kind == SyntaxKind.StarToken)
                    return left * right;
                else if(b.OperatorToken.Kind == SyntaxKind.SlashToken)
                    return left / right;
                else throw new Exception($"Unexpected binary operator {b.OperatorToken.Kind}");
            }
            if(root is ParenthesizedExpressionSyntax p){
                return EvaluateExpression(p.Expression);
            }
            else throw new Exception($"Unexpected node {root.Kind}");
        }
    }
}