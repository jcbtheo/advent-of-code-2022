using System;
using System.IO;

namespace AdventOfCode.Puzzles
{
    public class Day4 : IPuzzle
    {
        public void Solve()
        {
            SolveFirst();
        }

        private void SolveFirst()
        {
            var lines = File.ReadAllLines("inputs/day4.txt");

            var totalFullContains = 0;
            var totalOverlaps = 0;
            foreach(var line in lines)
            {
                var groupRanges = line.Split(",");

                var firstRange = groupRanges[0];
                var secondRange = groupRanges[1];

                if (RangesContainEachOther(firstRange, secondRange)) totalFullContains++;
                if (RangesOverlap(firstRange, secondRange)) totalOverlaps++;
            }

            Console.WriteLine(totalFullContains);
            Console.WriteLine(totalOverlaps);
        }

        private bool RangesContainEachOther(string firstRange, string secondRange)
        {
            var firstSplit = firstRange.Split("-");
            var firstMin = int.Parse(firstSplit[0]);
            var firstMax = int.Parse(firstSplit[1]);

            var secondSplit = secondRange.Split("-");
            var secondMin = int.Parse(secondSplit[0]);
            var secondMax = int.Parse(secondSplit[1]);

            var firstContainsSecond = firstMin <= secondMin && firstMax >= secondMax;
            var secondContainsFirst = secondMin <= firstMin && secondMax >= firstMax;

            return firstContainsSecond || secondContainsFirst;
        }

        private bool RangesOverlap(string firstRange, string secondRange)
        {
            var firstSplit = firstRange.Split("-");
            var firstMin = int.Parse(firstSplit[0]);
            var firstMax = int.Parse(firstSplit[1]);

            var secondSplit = secondRange.Split("-");
            var secondMin = int.Parse(secondSplit[0]);
            var secondMax = int.Parse(secondSplit[1]);

            var firstOverlapsSecond = (firstMin >= secondMin && firstMin <= secondMax) || (firstMax >= secondMin && firstMax <= secondMax);
            var secondOverlapsFirst = (secondMin >= firstMin && secondMin <= firstMax) || (secondMax >= firstMin && secondMax <= firstMax);

            return firstOverlapsSecond || secondOverlapsFirst;
        }
    }
}
