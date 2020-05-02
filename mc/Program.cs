using System;
using System.Collections.Generic;
using System.Linq;
using Minsk.Code;
namespace Minsk
{
    class Program
    {
        static void Main(string[] args)
        {
            bool showTree = false;
            while(true){
                Console.Write(">");
                var color = Console.ForegroundColor;
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
                if(showTree){
                    PrettyPrint(syntaxTree.Root);
                }
                
                if(syntaxTree.Diags.Any()){
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    foreach(var d in parser.Diagnostics){
                        Console.WriteLine(d);
                    }
                    Console.ForegroundColor = color;
                }else{
                    var e = new Evaluator(syntaxTree.Root);
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
            if(node is SyntaxToken t && t.value != null){
                Console.Write(" ");
                Console.Write(t.value);
            }
            Console.WriteLine();
            indent += isLast ? "    " : "│    ";
            var children = node.GetChildren();
            var lastChild = children.LastOrDefault();

            foreach(var child in children){
                PrettyPrint(child, indent, child == lastChild);
            }
        }
    }
    
    
}
