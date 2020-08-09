using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Minsk.CodeAnalysis.Syntax;

namespace Minsk.CodeAnalysis.Binding
{
    internal sealed class Binder{
        public Binder(BoundScope parent)
        {
            _scope = new BoundScope(parent);
        }
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
        private BoundScope _scope;
        public DiagnosticBag Diagnostics => _diagnostics;
        private static BoundScope CreateParentScopes(BoundGlobalScope previous){
            var stack = new Stack<BoundGlobalScope>();
            while(previous != null){
                stack.Push(previous);
                previous = previous.Previous;
            }
            BoundScope parent = null;
            while(stack.Count > 0){
                previous = stack.Pop();
                var scope = new BoundScope(parent);
                foreach(var v in previous.Variables)
                    scope.TryDeclare(v);
                parent = scope;
            }
            return parent;
        }

        public static BoundGlobalScope BindGlobalScope(BoundGlobalScope previous, CompilationUnitSyntax syntax){
            var parent = CreateParentScopes(previous);
            var binder = new Binder(parent);
            var expression = binder.BindStatement(syntax.Statement);
            var variables = binder._scope.GetDeclaredVariables();
            var diagnostics = binder.Diagnostics.ToImmutableArray();
            if(previous != null){
                diagnostics = diagnostics.InsertRange(0, previous.Diagnostics);
            }
            return new BoundGlobalScope(previous, diagnostics, variables, expression);
        }
        private BoundStatement BindStatement(StatementSyntax syntax){
            switch(syntax.Kind){
                case SyntaxKind.BlockStatement:
                    return BindBlockStatement((BlockStatementSyntax)syntax);
                case SyntaxKind.ExpressionStatement:
                    return BindExpressionStatement(((ExpressionStatementSyntax)syntax));
                case SyntaxKind.IfStatement:
                    return BindIfStatement(((IfStatementSyntax)syntax));
                case SyntaxKind.WhileStatement:
                    return BindWhileStatement(((WhileStatementSyntax)syntax));
                case SyntaxKind.ForStatement:
                    return BindForStatement(((ForStatementSyntax)syntax));
                case SyntaxKind.VariableDeclaration:
                    return BindVariableDeclaration(((VariableDeclarationSyntax)syntax));
                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");
            }
        }

        private BoundStatement BindForStatement(ForStatementSyntax syntax)
        {
            var lowerBound = BindExpression(syntax.LowerBound, typeof(int));
            var upperBound = BindExpression(syntax.UpperBound, typeof(int));
            _scope = new BoundScope(_scope);
            var name = syntax.Identifier.Text;
            var variable = new VariableSymbol(name, true, typeof(int));
            if(!_scope.TryDeclare(variable))
                _diagnostics.ReportVariableAlreadyDeclared(syntax.Identifier.Span, name);
            var body = BindStatement(syntax.Body);

            _scope = _scope.Parent;
            return new BoundForStatement(variable, lowerBound, upperBound, body);
        }

        private BoundStatement BindWhileStatement(WhileStatementSyntax syntax)
        {
            var condition = BindExpression(syntax.Condition, typeof(bool));
            var body = BindStatement(syntax.Body);
            return new BoundWhileStatement(condition, body);
        }

        private BoundStatement BindIfStatement(IfStatementSyntax syntax)
        {
            var condition = BindExpression(syntax.Condition, typeof(bool));
            var statement = BindStatement(syntax.ThenStatement);
            var elseStatement = syntax.ElseClauseSyntax is null 
                        ? null :BindStatement(syntax.ElseClauseSyntax.ElseStatement);
            return new BoundIfStatement(condition, statement, elseStatement);
        }

        private BoundExpression BindExpression(ExpressionSyntax syntax, Type type)
        {
            var result = BindExpression(syntax);
            if(result.Type != type)
                _diagnostics.ReportCannotConvert(syntax.Span, result.Type, type);
            return result;
        }

        private BoundStatement BindVariableDeclaration(VariableDeclarationSyntax syntax)
        {
            var expression = BindExpression(syntax.Initializer);
            var name = syntax.Identifier.Text;
            var isReadOnly = syntax.Keyword.Kind == SyntaxKind.LetKeyword;
            var initializer = BindExpression(syntax.Initializer);
            var variable = new VariableSymbol(name, isReadOnly, initializer.Type);
            if(!_scope.TryDeclare(variable)){
                _diagnostics.ReportVariableAlreadyDeclared(syntax.Identifier.Span, name);
            }
            return new BoundVariableDeclaration(variable, initializer);
        }

        private BoundStatement BindExpressionStatement(ExpressionStatementSyntax syntax)
        {
           var expression = BindExpression(syntax.Expression);
           return new BoundExpressionStatement(expression);
        }

        private BoundStatement BindBlockStatement(BlockStatementSyntax syntax)
        {
            var statements = ImmutableArray.CreateBuilder<BoundStatement>();
            _scope = new BoundScope(_scope);
            foreach(var statementSyntax in syntax.Statements){
                var statement = BindStatement(statementSyntax);
                statements.Add(statement);
            }
            _scope = _scope.Parent;
            return new BoundBlockStatement(statements.ToImmutable());
        }

        private BoundExpression BindExpression(ExpressionSyntax syntax){
            switch(syntax.Kind){
                case SyntaxKind.LiteralExpression:
                    return BindLiteralExpression((LiteralExpressionSyntax)syntax);
                case SyntaxKind.ParenthesizedExpression:
                    return BindParenthesizedExpression(((ParenthesizedExpressionSyntax)syntax));
                case SyntaxKind.NameExpression:
                    return BindNameExpression((NameExpressionSyntax)syntax);
                case SyntaxKind.AssignmentExpression:
                    return BindAssignmentExpression((AssignmentExpressionSyntax)syntax);
                case SyntaxKind.UnaryExpression:
                    return BindUnaryExpression((UnaryExpressionSyntax)syntax);
                case SyntaxKind.BinaryExpression:
                    return BindBinaryExpression((BinaryExpressionSyntax)syntax);
                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");
            }
        }
        private BoundExpression BindParenthesizedExpression(ParenthesizedExpressionSyntax syntax)
        {
            return BindExpression(syntax.Expression);
        }
        private BoundExpression BindAssignmentExpression(AssignmentExpressionSyntax syntax)
        {
            var name = syntax.IdentifierToken.Text;
            var boundExpression = BindExpression(syntax.Expression);
            if(!_scope.TryLookup(name, out var variable)){
                 _diagnostics.ReportUndefinedName(syntax.IdentifierToken.Span, name);
                return boundExpression;
            }
            if(variable.IsReadOnly){
                _diagnostics.ReportCannotAssign(syntax.EqualsToken.Span, name);
            }
            if(boundExpression.Type != variable.Type){
                _diagnostics.ReportCannotConvert(syntax.Expression.Span, boundExpression.Type, variable.Type);
                return boundExpression;
            }
           
            return new BoundAssignmentExpression(variable, boundExpression);
        }

        private BoundExpression BindNameExpression(NameExpressionSyntax syntax)
        {
            var name = syntax.IdentifierToken.Text;
            if(!_scope.TryLookup(name, out var variable)){
                _diagnostics.ReportUndefinedName(syntax.IdentifierToken.Span, name);
                return new BoundLiteralExpression(0);
            }
            return new BoundVariableExpression(variable);
        }

        private BoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
        {
            var boundOperand = BindExpression(syntax.Operand);
            var boundOperator = BoundUnaryOperator.Bind(syntax.OperatorToken.Kind, boundOperand.Type);
            if(boundOperator is null){
                _diagnostics.ReportUndefinedUnaryOperator(syntax.OperatorToken.Span, syntax.OperatorToken.Text, boundOperand.Type); 
                return boundOperand;
            }
            return new BoundUnaryExpression(boundOperator, boundOperand);
        }
        private BoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
        {
            var boundLeft = BindExpression(syntax.Left);
            var boundRight = BindExpression(syntax.Right);
            var boundOperator = BoundBinaryOperator.Bind(syntax.OperatorToken.Kind, boundLeft.Type, boundRight.Type);
           if(boundOperator is null){
               _diagnostics.ReportUndefinedBinaryOperator(syntax.OperatorToken.Span, syntax.OperatorToken.Text, boundLeft.Type, boundRight.Type); 
                return boundLeft;
            }
            return new BoundBinaryExpression(boundLeft, boundOperator, boundRight);
        }
        private BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
        {
            var value = syntax.Value ?? 0;
            return new BoundLiteralExpression(value);
        }
    }
}