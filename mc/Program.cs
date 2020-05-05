using System;
using System.Collections.Generic;
using System.Linq;
using Minsk.CodeAnalysis;
using Minsk.CodeAnalysis.Binding;
using Minsk.CodeAnalysis.Syntax;
namespace Minsk
{
    class Program
    {
        internal static void Main(string[] args)
        {
            bool showTree = false;
            while(true){
                Console.Write(">");
                var line = Console.ReadLine();
                if(string.IsNullOrWhiteSpace(line)){
                    return;
                }

                if(line.Equals("#showtree")){
                    showTree = !showTree;
                    Console.WriteLine(showTree ? "Showing parse trees.": "Not showing parse trees.");
                    continue;
                }else if (line.Equals("#cls")){
                    Console.Clear();
                    continue;
                }
                var syntaxTree = SyntaxTree.Parse(line);
                var compilation = new Compilation(syntaxTree);
                var result = compilation.Evaluate();

                if(showTree){
                    PrettyPrint(syntaxTree.Root);
                }
                if(!result.Diagnostics.Any()){
                    Console.WriteLine(result.Value);
                }else{
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    foreach(var d in result.Diagnostics){
                        Console.WriteLine();

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(d);
                        Console.ResetColor();

                        var prefix = line.Substring(0, d.Span.Start);
                        var error = line.Substring(d.Span.Start, d.Span.Length);
                        var suffix = line.Substring(d.Span.End);
                        
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

        static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true){
            var marker = isLast ? "└──" : "├──";
            Console.Write(indent);
            Console.Write(marker);
            Console.Write(node.Kind);
            if(node is SyntaxToken t && t.Value != null){
                Console.Write(" ");
                Console.Write(t.Value);
            }
            Console.WriteLine();
            indent += isLast ? "   " : "│  ";
            var children = node.GetChildren();
            var lastChild = children.LastOrDefault();

            foreach(var child in children){
                PrettyPrint(child, indent, child == lastChild);
            }
        }
    }
    
    
}
