using System;
using System.IO;
using System.Linq;

namespace AdventOfCode.Puzzles
{
    public class Day3 : IPuzzle
    {
        public void Solve()
        {
            SolveFirst();
            SolveSecond();
        }

        private void SolveFirst()
        {
            var totalPriority = 0;

            var lines = File.ReadAllLines("inputs/day3.txt");
            foreach (var line in lines)
            {
                var firstHalf = line.Substring(0, line.Length / 2);
                var secondHalf = line.Substring(line.Length / 2, line.Length / 2);

                var common = firstHalf.Intersect(secondHalf);
                foreach (var c in common)
                {
                    totalPriority += GetCharIntValue(c);
                }
            }

            Console.WriteLine(totalPriority);
        }

        private void SolveSecond()
        {
            var totalPriority = 0;

            var lines = File.ReadAllLines("inputs/day3.txt").ToList();
             
            var elfGroupIndex = 0;
            var elfGroupSize = 3;
            while(elfGroupIndex <= lines.Count - elfGroupSize)
            {
                var elfGroupLines = lines.GetRange(elfGroupIndex, 3);

                var firstIntersection = elfGroupLines[0].Intersect(elfGroupLines[1]);
                var firstIntersectionString = new string(firstIntersection.ToArray());

                var common = firstIntersectionString.Intersect(elfGroupLines[2]);
                foreach (var c in common)
                {
                    totalPriority += GetCharIntValue(c);
                }

                elfGroupIndex += elfGroupSize;
            }
            
            Console.WriteLine(totalPriority);
        }

        private int GetCharIntValue(char c)
        {
            if (char.IsUpper(c))
            {
                return c - 38; // offset of ascii int value to start at 27 for A
            }
            else
            {
                return c - 96; // offset of ascii int value to start at 1 for a
            }
        }
    }
}
