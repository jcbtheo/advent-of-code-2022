using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Puzzles
{
    public class Day2 : IPuzzle
    {
        private const int rock = 1;
        private const int paper = 2;
        private const int scissors = 3;

        private const int loss = 0;
        private const int draw = 3;
        private const int win = 6;

        private readonly Dictionary<string, int> RockPaperScissorsTable =
        new Dictionary<string, int>
        {
            {"A",  rock},
            {"B",  paper}, 
            {"C",  scissors},
            {"X",  rock},
            {"Y",  paper},
            {"Z",  scissors},
        };

        private readonly Dictionary<string, int> WinLossDrawTable =
        new Dictionary<string, int>
        {
            {"X",  loss},
            {"Y",  draw},
            {"Z",  win},
        };

        public void Solve()
        {
            SolveFirst();
            SolveSecond();
        }

        private void SolveFirst()
        {
            var lines = File.ReadAllLines("inputs/day2.txt");

            var myPoints = 0;

            foreach (var line in lines)
            {
                var playedValues = line.Split(" ");
                var opponentValue = RockPaperScissorsTable[playedValues[0]];
                var myValue = RockPaperScissorsTable[playedValues[1]];

                myPoints += GetWinLossOrDraw(myValue, opponentValue);
            }

            Console.WriteLine(myPoints);
        }

        private int GetWinLossOrDraw(int myValue, int opponentValue)
        {
            if (myValue == opponentValue)
            {
                return myValue + draw;
            }

            switch (myValue)
            {
                case rock:
                    if (opponentValue == scissors) return myValue + win;
                    return myValue + loss;
                case paper:
                    if (opponentValue == rock) return myValue + win;
                    return myValue + loss;
                case scissors:
                    if (opponentValue == paper) return myValue + win;
                    return myValue + loss;
                default:
                    throw new Exception("unknown move");
            }
        }

        private void SolveSecond()
        {
            var lines = File.ReadAllLines("inputs/day2.txt");

            var myPoints = 0;

            foreach (var line in lines)
            {
                var playedValues = line.Split(" ");
                var opponentValue = RockPaperScissorsTable[playedValues[0]];
                var myValue = WinLossDrawTable[playedValues[1]];

                myPoints += ForceWinLossOrDraw(myValue, opponentValue);
            }

            Console.WriteLine(myPoints);
        }

        private int ForceWinLossOrDraw(int myValue, int opponentValue)
        {
            switch (myValue)
            {
                case loss:
                    if (opponentValue == rock) return scissors + loss;
                    else if (opponentValue == paper) return rock + loss;
                    return paper + loss;
                case draw:
                    return opponentValue + draw;
                case win:
                    if (opponentValue == rock) return paper + win;
                    else if (opponentValue == paper) return scissors + win;
                    return rock + win;
                default:
                    throw new Exception("unknown move");
            }
        }
    }
}
