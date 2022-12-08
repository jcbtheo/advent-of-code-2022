using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Puzzles
{
    public class Day8 : IPuzzle
    {
        public void Solve()
        {
            var lines = File.ReadAllLines("inputs/day8.txt");
            var map = InitializeMap(lines);

            CalculateTotalVisibleTrees(map);
            DetermineHighestScenicScore(map);
        }

        private void DetermineHighestScenicScore(int[,] map)
        {
            var length = map.GetLength(0);
            var maxScenicScore = 0;

            for (int rowIndex = 1; rowIndex < length - 1; rowIndex++)
            {
                for (int columnIndex = 1; columnIndex < length - 1; columnIndex++)
                {
                    var value = map[rowIndex, columnIndex];
                    var row = GetRow(map, rowIndex);
                    var column = GetColumn(map, columnIndex);

                    var leftRightScore = GetLeftAndRightScenicScore(row, columnIndex, value);
                    var topBottomScore = GetTopAndBottomScenicScore(column, rowIndex, value);

                    maxScenicScore = Math.Max(maxScenicScore, leftRightScore * topBottomScore);
                }
            }

            Console.WriteLine(maxScenicScore);
        }

        private int GetLeftAndRightScenicScore(int[] row, int index, int value)
        {
            var leftRange = row.Take(index).Reverse();
            var rightRange = row.Skip(index + 1);

            return GetScenicScore(leftRange, value) * GetScenicScore(rightRange, value);
        }

        private int GetTopAndBottomScenicScore(int[] col, int index, int value)
        {
            var topRange = col.Take(index).Reverse();
            var bottomRange = col.Skip(index + 1);

            return GetScenicScore(topRange, value) * GetScenicScore(bottomRange, value);
        }

        private int GetScenicScore(IEnumerable<int> heights, int value)
        {
            var score = 0;
            foreach (var height in heights)
            {
                if (height >= value)
                {
                    score++;
                    break;
                }
                score++;
            }

            return score;
        }

        private void CalculateTotalVisibleTrees(int[,] map)
        {
            var length = map.GetLength(0);
            var visible = (length * 2) + ((length - 2) * 2);

            for (int rowIndex = 1; rowIndex < length - 1; rowIndex++)
            {
                for (int columnIndex = 1; columnIndex < length - 1; columnIndex++)
                {
                    var value = map[rowIndex, columnIndex];
                    var row = GetRow(map, rowIndex);
                    var column = GetColumn(map, columnIndex);

                    if (IsVisibleFromLeftOrRight(row, columnIndex, value)
                        || IsVisibleFromTopOrBottom(column, rowIndex, value))
                    {
                        visible++;
                    }
                }
            }

            Console.WriteLine(visible);
        }

        private bool IsVisibleFromLeftOrRight(int[] row, int index, int value)
        {
            var leftRange = row.Take(index).All(x => x < value);
            var rightRange = row.Skip(index + 1).All(x => x < value);

            return leftRange || rightRange;
        }

        private bool IsVisibleFromTopOrBottom(int[] col, int index, int value)
        {
            var topRange = col.Take(index).All(x => x < value);
            var bottomRange = col.Skip(index + 1).All(x => x < value);

            return topRange || bottomRange;
        }

        private int[,] InitializeMap(string[] lines)
        {
            var map = new int[lines.Length, lines.Length];

            for (var i = 0; i < lines.Length; i++)
            {
                var heights = lines[i];

                for (var h = 0; h < heights.Length; h++)
                {
                    map[i, h] = int.Parse(heights[h].ToString());
                }
            }

            return map;
        }

        private int[] GetColumn(int[,] matrix, int columnNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(0))
                    .Select(x => matrix[x, columnNumber])
                    .ToArray();
        }

        private int[] GetRow(int[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                    .Select(x => matrix[rowNumber, x])
                    .ToArray();
        }
    }
}
