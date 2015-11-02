using System;
using EQL_AbstractionPost;

namespace EightQueensConsole
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var solver = new EightQueensSolver();
            var result = solver.Solve();
            foreach (var tuple in result)
            {
                Console.WriteLine(tuple.Item1 + " " + tuple.Item2);
            }

        }
    }
}
