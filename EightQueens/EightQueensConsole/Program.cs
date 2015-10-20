using System;
using EQL;

namespace EightQueensConsole
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Solver solver = new Solver();
            var result = solver.Solve();
            foreach (var tuple in result)
            {
                Console.WriteLine(tuple.Item1 + " " + tuple.Item2);
            }

        }
    }
}
