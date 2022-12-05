using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles
{
    public class Instruction
    {
        public int Move { get; set; }
        public int From { get; set; }
        public int To { get; set; }
    }

    public class Day5 : IPuzzle
    {
        public void Solve()
        {
            SolveFirst();
            SolveSecond();
        }

        private void SolveFirst()
        {
            var stack = GetStartingStacks();
            var instructions = GetInstructions();

            ApplyInstructions(stack, instructions, true);

            var result = string.Join("", stack.Select(x => x[0]));
            Console.WriteLine(result);
        }

        private void SolveSecond()
        {
            var stack = GetStartingStacks();
            var instructions = GetInstructions();

            ApplyInstructions(stack, instructions);

            var result = string.Join("", stack.Select(x => x[0]));
            Console.WriteLine(result);
        }

        private void ApplyInstructions(List<List<string>> stack, List<Instruction> instructions, bool reverse = false)
        {
            foreach (var instruction in instructions)
            {
                var itemsToMove = stack[instruction.From].GetRange(0, instruction.Move);

                if (reverse)
                {
                    itemsToMove.Reverse();
                }

                stack[instruction.To].InsertRange(0, itemsToMove);
                stack[instruction.From].RemoveRange(0, instruction.Move);
            }
        }

        private List<List<string>> GetStartingStacks()
        {
            var lines = File.ReadAllLines("inputs/day5.txt");
            var initialStack = new List<List<string>>();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) break; // instructions separation

                var row = line.Split(" ");
                if (row[1] == "1") break; // label for crates

                var cleanRow = new List<string>();
                for (var i = 0; i < row.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(row[i]))
                    {
                        cleanRow.Add(string.Empty);
                        i += 3;
                    }
                    else
                    {
                        cleanRow.Add(row[i]);
                    }
                }

                for (var i = 0; i < cleanRow.Count; i++)
                {
                    if (cleanRow.Count > initialStack.Count)
                    {
                        initialStack.Add(new List<string>());
                    }

                    if (!string.IsNullOrWhiteSpace(cleanRow[i]))
                    {
                        initialStack[i].Add(cleanRow[i]);
                    }
                }
            }

            return initialStack;
        }

        private List<Instruction> GetInstructions()
        {
            var instructions = new List<Instruction>();

            Regex instructionRegex = new Regex(@"move (\d+) from (\d+) to (\d+)");

            var isInstruction = false;
            var lines = File.ReadAllLines("inputs/day5.txt");
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line)) // instruction separation
                {
                    isInstruction = true;
                    continue;
                }
                if (!isInstruction) continue;

                var match = instructionRegex.Match(line);
                var groups = match.Groups;

                instructions.Add(new Instruction
                {
                    Move = int.Parse(groups[1].Value),
                    From = int.Parse(groups[2].Value) -1,
                    To = int.Parse(groups[3].Value) - 1,
                });
            }

            return instructions;
        }
    }
}
