using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Puzzles
{
    public class Day10 : IPuzzle
    {
        private readonly List<int> _signals = new List<int>();
        private readonly string[,] _screen = new string[6,40];

        public void Solve()
        {
            var clockCycle = 1;
            var xRegister = 1;
            var xInstructionActive = false;

            using var reader = new StreamReader("inputs/day10.txt");
            var line = reader.ReadLine();
            while (line != null)
            {
                UpdateSignals(clockCycle, xRegister);
                UpdateScreen(clockCycle, xRegister);

                clockCycle++;

                var xInstructionValue = ParseInstruction(line);
                if (xInstructionValue == null)
                {
                    line = reader.ReadLine();
                    continue;
                }

                xInstructionActive = !xInstructionActive;
                if (!xInstructionActive)
                {
                    xRegister += xInstructionValue.Value;
                    line = reader.ReadLine();
                }
            }

            Console.WriteLine(_signals.Sum(x => x));
            PrintScreen();
        }

        private void UpdateSignals(int clockCycle, int xRegister)
        {
            if (clockCycle >= 20 && (clockCycle - 20) % 40 == 0)
            {
                _signals.Add(clockCycle * xRegister);
            }
        }

        private void UpdateScreen(int clockCycle, int xRegister)
        {
            var row = (clockCycle - 1) / 40;
            var spriteCenterCol = xRegister;
            var clockCycleCol = clockCycle - (40 * row) - 1;

            _screen[row, clockCycleCol] = IsSpriteVisible(spriteCenterCol, clockCycleCol) ? "#" : ".";
        }

        private bool IsSpriteVisible(int spriteCenterCol, int clockCycleCol)
        {
            return Enumerable
                .Range(spriteCenterCol - 1, 3)
                .Contains(clockCycleCol);
        }

        private void PrintScreen()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    Console.Write(_screen[i, j]);
                }
                Console.WriteLine();
            }
        }

        private int? ParseInstruction(string line)
        {
            if (line.StartsWith("addx"))
            {
                return int.Parse(line.Split(" ")[1]);
            }

            return null;
        }
    }
}
