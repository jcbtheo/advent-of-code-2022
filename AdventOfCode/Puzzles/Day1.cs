using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Puzzles
{
    public class Day1 : IPuzzle
    {
        public void Solve()
        {
            SolveFirst();
            SolveSecond();
        }

        public void SolveFirst()
        {
            var max = 0;
            var elfTotal = 0;

            var lines = File.ReadAllLines("inputs/day1.txt");
            foreach (var line in lines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    elfTotal += int.Parse(line);
                }
                else
                {
                    max = Math.Max(max, elfTotal);
                    elfTotal = 0;
                }
            }

            Console.WriteLine(max);
        }

        public void SolveSecond()
        {
            var elfTotals = new List<int>();
            var elfTotal = 0;

            var lines = File.ReadAllLines("inputs/day1.txt");
            foreach (var line in lines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    elfTotal += int.Parse(line);
                }
                else
                {
                    elfTotals.Add(elfTotal);
                    elfTotal = 0;
                }
            }

            var total = elfTotals
                .OrderByDescending(x => x)
                .Take(3)
                .Sum();

            Console.WriteLine(total);
        }
    }
}
