using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Puzzles
{
    public class Monkey
    {
        public List<long> Items { get; set; }
        public Func<long, long> Operation { get; set; }
        public int DivisibleCheck { get; set; }
        public int TrueMonkey { get; set; }
        public int FalseMonkey { get; set; }
        public long InspectionCount { get; set; } = 0;
    }

    public class Day11 : IPuzzle
    {
        public void Solve()
        {
            var lines = File.ReadAllLines("inputs/day11.txt");
            SolveFirst(lines);
            SolveSecond(lines);
        }

        // TODO: clean this up to reduce all the duplicate code
        private void SolveSecond(string[] lines)
        {
            var monkies = CreateStartingMonkeys(lines);

            // modulo arithmetic, maybe chinese remainder theorem? 
            var divisorsLCM = monkies.Aggregate(1, (x, y) => x * y.DivisibleCheck);

            var round = 0;
            while (round++ < 10000)
            {
                foreach (var monkey in monkies)
                {
                    foreach (var item in monkey.Items)
                    {
                        monkey.InspectionCount++;

                        var worryLevel = monkey.Operation(item);
                        worryLevel %= divisorsLCM;

                        var monkeyToPassTo = worryLevel % monkey.DivisibleCheck == 0 ? monkey.TrueMonkey : monkey.FalseMonkey;

                        monkies[monkeyToPassTo].Items.Add(worryLevel);
                    }

                    monkey.Items.Clear();
                }
            }

            foreach (var monkey in monkies)
            {
                Console.WriteLine("inspection count: " + monkey.InspectionCount);
            }

            var total = monkies.OrderByDescending(x => x.InspectionCount).Select(x => x.InspectionCount).Take(2).ToList();
            Console.WriteLine("Total: " + total[0] * total[1]);
        }

        private void SolveFirst(string[] lines)
        {
            var monkies = CreateStartingMonkeys(lines);

            var round = 0;
            while (round++ < 20)
            {
                foreach (var monkey in monkies)
                {
                    foreach (var item in monkey.Items)
                    {
                        monkey.InspectionCount++;

                        var worryLevel = monkey.Operation(item) / 3;

                        var monkeyToPassTo = worryLevel % monkey.DivisibleCheck == 0 ? monkey.TrueMonkey : monkey.FalseMonkey;

                        monkies[monkeyToPassTo].Items.Add(worryLevel);
                    }

                    monkey.Items.Clear();
                }
            }

            foreach (var monkey in monkies)
            {
                Console.WriteLine("inspection count: " + monkey.InspectionCount);
            }

            var total = monkies.OrderByDescending(x => x.InspectionCount).Select(x => x.InspectionCount).Take(2).ToList();
            Console.WriteLine("Total: " + total[0] * total[1]);
        }

        private List<Monkey> CreateStartingMonkeys(string[] lines)
        {
            var monkies = new List<Monkey>();

            var monkeyInputs = lines
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / 6)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();

            foreach (var input in monkeyInputs)
            {
                var monkey = new Monkey
                {
                    Items = ParseStartingItems(input[1]),
                    Operation = ParseOperation(input[2]),
                    DivisibleCheck = ParseDivisibleBy(input[3]),
                    TrueMonkey = ParseMonkey(input[4]),
                    FalseMonkey = ParseMonkey(input[5]),
                };

                monkies.Add(monkey);
            }

            return monkies;
        }

        private List<long> ParseStartingItems(string input)
        {
            var numbers = input
                .Split(":")[1]
                .Split(", ")
                .Select(x => long.Parse(x))
                .ToList();

            return numbers;
        }

        private Func<long, long> ParseOperation(string input)
        {
            var split = input
                .Split("new = old ");

            var operationComponents = split[1].Split(" ");

            var symbol = operationComponents[0];
            var isNumber = int.TryParse(operationComponents[1], out var number);

            if (symbol == "+")
            {
                if (isNumber)
                {
                    return x => x + number;
                }
                return x => x + x;
            }

            if (isNumber)
            {
                return x => x * number;
            }
            return x => x * x;
        }

        private int ParseDivisibleBy(string input)
        {
            var split = input
                .Split("by ");

            return int.Parse(split[1]);
        }

        private int ParseMonkey(string input)
        {
            var split = input
                .Split("monkey");

            return int.Parse(split[1]);
        }
    }
}

