using AdventOfCode.Puzzles;
using System;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            GetPuzzleInstanceForDay(1).Solve();
        }

        private static IPuzzle GetPuzzleInstanceForDay(int day)
        {
            var t = Type.GetType($"AdventOfCode.Puzzles.Day{day}");
            IPuzzle instance = (IPuzzle)Activator.CreateInstance(t);

            Console.WriteLine($"Day {day}:");

            return instance;
        }
    }
}
