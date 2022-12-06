using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Puzzles
{
    public class Day6 : IPuzzle
    {
        public void Solve()
        {
            var lines = File.ReadAllLines("inputs/day6.txt");
            var line = lines[0];

            FindMarkerIndex(line, 4);
            FindMarkerIndex(line, 14);
        }

        private void FindMarkerIndex(string input, int markerSize)
        {
            var index = 0;
            while (index + markerSize <= input.Length)
            {
                var substring = input.Substring(index, markerSize);

                if (!HasDuplicates(substring))
                {
                    break;
                }

                index++;
            }

            Console.WriteLine(index + markerSize);
        }

        private bool HasDuplicates(string packetMarker)
        {
            var enteredCharacters = new List<char>();

            foreach(var c in packetMarker)
            {
                if (enteredCharacters.Contains(c))
                {
                    return true;
                }
                enteredCharacters.Add(c);
            }

            return false;
        }
    }
}
