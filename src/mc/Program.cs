using Minsk.CodeAnalysis.Binding;

namespace Minsk
{
    class Program
    {
        internal static void Main(string[] args)
        {
            var repl = new MinskRepl();
            repl.Run();
        }
    }
}
