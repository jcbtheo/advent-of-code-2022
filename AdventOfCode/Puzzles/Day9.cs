using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Puzzles
{
    public class Point
    {
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public bool IsHead { get; set; } = false;
        public bool IsTail { get; set; } = false;
    }

    public class Day9 : IPuzzle
    {
        public void Solve()
        {
            SolveForLength(2);
            SolveForLength(10);
        }

        private void SolveForLength(int length)
        {
            var visited = new HashSet<Tuple<int, int>>();
            var lines = File.ReadAllLines("inputs/day9.txt");

            var points = Enumerable.Range(0, length).Select(x => new Point()).ToList();
            points[0].IsHead = true;
            points[^1].IsTail = true;

            foreach (var line in lines)
            {
                UpdatePoints(points, line, visited);
            }

            Console.WriteLine(visited.Count);
        }

        private void UpdatePoints(List<Point> points, string line, HashSet<Tuple<int, int>> visited)
        {
            var items = line.Split(" ");
            var direction = items[0];
            var distance = int.Parse(items[1]);

            for (var i = 0; i < distance; i++)
            {
                for (var p = 0; p < points.Count - 1; p++)
                {
                    var currentPoints = points.GetRange(p, 2);
                    var leadPoint = currentPoints[0];
                    var followPoint = currentPoints[1];

                    if (leadPoint.IsHead)
                    {
                        if (direction == "R")
                        {
                            leadPoint.X += 1;
                        }
                        else if (direction == "L")
                        {
                            leadPoint.X -= 1;
                        }
                        else if (direction == "U")
                        {
                            leadPoint.Y += 1;
                        }
                        else if (direction == "D")
                        {
                            leadPoint.Y -= 1;
                        }
                    }

                    UpdateTrailingPointLocation(leadPoint, followPoint, visited);
                }
            }
        }

        private void UpdateTrailingPointLocation(Point leadPoint, Point followPoint, HashSet<Tuple<int, int>> visited)
        {
            var xDifference = leadPoint.X - followPoint.X;
            var yDifference = leadPoint.Y - followPoint.Y;

            if (Math.Abs(xDifference) > 1)
            {
                followPoint.X += xDifference / 2;

                if (Math.Abs(yDifference) > 0)
                {
                    followPoint.Y += yDifference > 0 ? 1 : -1;
                }
            }
            else if (Math.Abs(yDifference) > 1)
            {
                followPoint.Y += yDifference / 2;

                if (Math.Abs(xDifference) > 0)
                {
                    followPoint.X += xDifference > 0 ? 1 : -1;
                }
            }

            if (followPoint.IsTail)
            {
                visited.Add(Tuple.Create(followPoint.X, followPoint.Y));
            }
        }
    }
}
