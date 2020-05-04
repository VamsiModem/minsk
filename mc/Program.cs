using System;
using System.Collections.Generic;
using System.Linq;
using Minsk.Code;
using Minsk.Code.Binding;
using Minsk.Code.Syntax;
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
                var parser = new Parser(line);
                var syntaxTree = parser.Parse();
                var binder = new Binder();
                var boundExpression = binder.BindExpression(syntaxTree.Root);

                if(showTree){
                    PrettyPrint(syntaxTree.Root);
                }
                var diagnostics = syntaxTree.Diagnostics.Concat(binder.Diagnostics);
                if(diagnostics.Any()){
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    foreach(var d in diagnostics){
                        Console.WriteLine(d);
                    }
                    Console.ResetColor();
                }else{
                    var e = new Evaluator(boundExpression);
                    var result = e.Evaluate();
                    Console.WriteLine(result);
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
