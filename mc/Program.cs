using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Minsk.CodeAnalysis;
using Minsk.CodeAnalysis.Binding;
using Minsk.CodeAnalysis.Syntax;
using Minsk.CodeAnalysis.Text;

namespace Minsk
{
    class Program
    {
        internal static void Main(string[] args)
        {
            bool showTree = false;
            var variables = new Dictionary<VariableSymbol, object>();
            var textBuilder = new StringBuilder();
            while(true){
                Console.ForegroundColor = ConsoleColor.Green;
                if(textBuilder.Length == 0)
                    Console.Write("» ");
                else
                    Console.Write("· ");

                Console.ResetColor();
                
                var input = Console.ReadLine();
                var isBlank = string.IsNullOrWhiteSpace(input);
                if(textBuilder.Length == 0){
                    if(isBlank){
                        break;
                    }
                    else if(input.Equals("#showtree")){
                        showTree = !showTree;
                        Console.WriteLine(showTree ? "Showing parse trees.": "Not showing parse trees.");
                        continue;
                    }else if (input.Equals("#cls")){
                        Console.Clear();
                        continue;
                    }
                    
                }
                textBuilder.AppendLine(input);
                var text = textBuilder.ToString();
                var syntaxTree = SyntaxTree.Parse(text);

                if(!isBlank && syntaxTree.Diagnostics.Any()) 
                    continue;


                var compilation = new Compilation(syntaxTree);
                var result = compilation.Evaluate(variables);

                if(showTree){
                    syntaxTree.Root.WriteTo(Console.Out);
                }
                if(!result.Diagnostics.Any()){
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(result.Value);
                    Console.ResetColor();
                }else{
                    foreach(var d in result.Diagnostics){
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
                textBuilder.Clear();
            }
        }
    } 
}
