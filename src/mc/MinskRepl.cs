using System;
using System.Collections.Generic;
using System.Linq;
using Minsk.CodeAnalysis;
using Minsk.CodeAnalysis.Syntax;
using Minsk.CodeAnalysis.Text;

namespace Minsk
{
    internal sealed class MinskRepl : Repl
    {

        private bool _showTree = false;
        private bool _showProgram = false;
        private Compilation _previous = null;
        private readonly Dictionary<VariableSymbol, object> _variables = new Dictionary<VariableSymbol, object>();
        protected override bool IsCompleteSubmission(string text)
        {
            if (string.IsNullOrEmpty(text))
                return true;

            var syntaxTree = SyntaxTree.Parse(text);
            if (syntaxTree.Diagnostics.Any())
                return false;

            return true;
        }


        protected override void EvaluateMetaCommand(string input)
        {
            switch (input)
            {
                case "#showTree":
                    _showTree = !_showTree;
                    Console.WriteLine(_showTree ? "Showing parse trees." : "Not showing parse trees.");
                    break;
                case "#showProgram":
                    _showProgram = !_showProgram;
                    Console.WriteLine(_showProgram ? "Showing bound trees." : "Not showing bound trees.");
                    break;
                case "#cls":
                    Console.Clear();
                    break;
                case "#reset":
                    _previous = null;
                    _variables.Clear();
                    break;
                default:
                    base.EvaluateMetaCommand(input);
                    break;
            }
        }

        protected override void EvaluateSubmission(string text)
        {
            var syntaxTree = SyntaxTree.Parse(text);
            var compilation = _previous is null ? new Compilation(syntaxTree) : _previous.ContinueWith(syntaxTree);
            if (_showTree)
                syntaxTree.Root.WriteTo(Console.Out);
            if (_showProgram)
                compilation.EmitTree(Console.Out);
            var result = compilation.Evaluate(_variables);
            if (!result.Diagnostics.Any())
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(result.Value);
                Console.ResetColor();
                _previous = compilation;
            }
            else
            {
                foreach (var d in result.Diagnostics)
                {
                    var lineIndex = syntaxTree.Text.GetLineIndex(d.Span.Start);
                    var lineNumber = lineIndex + 1;
                    var line = syntaxTree.Text.Lines[lineIndex];
                    var character = d.Span.Start - line.Start + 1;
                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write($"({lineNumber}, {character}): ");
                    Console.WriteLine(d);
                    Console.ResetColor();

                    var prefixSpan = TextSpan.FromBounds(line.Start, d.Span.Start);
                    var suffixSpan = TextSpan.FromBounds(d.Span.End, line.End);
                    var prefix = syntaxTree.Text.ToString(prefixSpan);
                    var error = syntaxTree.Text.ToString(d.Span);
                    var suffix = syntaxTree.Text.ToString(suffixSpan);

                    Console.Write("   ");
                    Console.Write(prefix);
                    Console.ForegroundColor = ConsoleColor.DarkRed;

                    Console.Write(error);
                    Console.ResetColor();
                    Console.Write(suffix);
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }
    }
}
