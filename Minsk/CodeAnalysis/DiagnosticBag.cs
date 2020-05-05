using System;
using System.Collections;
using System.Collections.Generic;
using Minsk.CodeAnalysis.Syntax;

namespace Minsk.CodeAnalysis
{
    internal sealed class DiagnosticBag: IEnumerable<Diagnostic>
    {
        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();
        public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void AddRange(DiagnosticBag diagnostics) => _diagnostics.AddRange(diagnostics);
        private void Report(TextSpan span, string message){
            var diagnostic = new Diagnostic(span, message);
            _diagnostics.Add(diagnostic);
        }
        public void ReportInvalidNumber(TextSpan textSpan, string text, Type type)
        {
            var message = $"The number {text} isn't a valid {type}.";
            Report(textSpan, message);
        }

        public void ReportBadCharacter(int position, char character)
        {
            var span = new TextSpan(position, 1);
            var message = $"Bad character input: {character}.";
            Report(span, message);
        }

        public void ReportUnexpectedToken(TextSpan span, SyntaxKind autualKind, SyntaxKind expectedKind)
        {
            var message = $"Unexpected token <{autualKind}>, expected <{expectedKind}>";
            Report(span, message);
        }

        public void ReportUndefinedUnaryOperator(TextSpan span, string operatorText, Type operandType)
        {
            var message = $"Unary operator '{operatorText}' is not defined for type {operandType}";
            Report(span, message);
        }

        public  void ReportUndefinedBinaryOperator(TextSpan span, string operatorText, Type leftType, Type rightType)
        {
             var message = $"Binary operator '{operatorText}' is not defined for types {leftType} and  {leftType}";
            Report(span, message);
        }
    }
}