using System;
using System.Collections.Generic;
using Minsk.CodeAnalysis.Binding;
namespace Minsk.CodeAnalysis
{
    internal sealed class Evaluator
    {
        private readonly BoundStatement _root;
        private readonly Dictionary<VariableSymbol, object> _variables;
        private object _lastValue;
        public Evaluator(BoundStatement root, Dictionary<VariableSymbol, object> variables)
        {
            _root = root;
            _variables = variables;
        }

        public object Evaluate(){
             EvaluateStatement(_root);
             return _lastValue;
        }
        private void EvaluateStatement(BoundStatement node)
        {
            switch (node.Kind)
            {
                case BoundNodeKind.BlockStatement:
                    EvaluateBlockStatement((BoundBlockStatement)node);
                    break;
                case BoundNodeKind.ExpressionStatement:
                    EvaluateExpressionStatement((BoundExpressionStatement)node);
                    break;
                default:
                    throw new Exception($"Unexpected node {node.Kind}");
            }
        }

        private void EvaluateExpressionStatement(BoundExpressionStatement node)
        {
            _lastValue = EvaluateExpression(node.Expression);
        }

        private void EvaluateBlockStatement(BoundBlockStatement node)
        {
            foreach(var statement in node.Statements)
                EvaluateStatement(statement);
        }

        private object EvaluateExpression(BoundExpression root)
        {
            switch (root.Kind)
            {
                case BoundNodeKind.LiteralExpression:
                    return EvaluateLiteralExpression((BoundLiteralExpression)root);
                case BoundNodeKind.VariableExpression:
                    return EvaluateVariableExpression((BoundVariableExpression)root);
                case BoundNodeKind.AssignmentExpression:
                    return EvaluateAssignmentExpression((BoundAssignmentExpression)root);
                case BoundNodeKind.UnaryExpression:
                    return EvaluateUnaryExpression((BoundUnaryExpression)root);
                case BoundNodeKind.BinaryExpression:
                    return EvalauteBinaryExpression((BoundBinaryExpression)root);
                default:
                    throw new Exception($"Unexpected node {root.Kind}");
            }
        }

        private object EvalauteBinaryExpression(BoundBinaryExpression b)
        {
            var left = EvaluateExpression(b.Left);
            var right = EvaluateExpression(b.Right);
            switch (b.Op.Kind)
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
                case BoundBinaryOperatorKind.Equals:
                    return Equals(left, right);
                case BoundBinaryOperatorKind.NotEquals:
                    return !Equals(left, right);
                default:
                    throw new Exception($"Unexpected binary operator {b.Op.Kind}");
            }
        }

        private object EvaluateUnaryExpression(BoundUnaryExpression u)
        {
            var operand = EvaluateExpression(u.Operand);
            switch (u.Op.Kind)
            {
                case BoundUnaryOperatorKind.Identity:
                    return (int)operand;
                case BoundUnaryOperatorKind.Negation:
                    return -(int)operand;
                case BoundUnaryOperatorKind.LogicalNegation:
                    return !(bool)operand;
                default:
                    throw new Exception($"Unexpected binary operator {u.Op.Kind}");
            }
        }

        private object EvaluateAssignmentExpression(BoundAssignmentExpression a)
        {
            var value = EvaluateExpression(a.Expression);
            _variables[a.Variable] = value;
            return value;
        }

        private object EvaluateVariableExpression(BoundVariableExpression v)
        {
            return _variables[v.Variable];
        }

        private static object EvaluateLiteralExpression(BoundLiteralExpression n)
        {
            return n.Value;
        }
    }
}