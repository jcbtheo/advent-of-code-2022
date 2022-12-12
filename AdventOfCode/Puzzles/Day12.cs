using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Puzzles
{
    public class Node
    {
        public (int, int) Coordinates { get; set; }
        public char Value { get; set; }
        public Node Parent { get; set; }
        public bool Explored { get; set; }

        public Node((int, int) coordinates, char value)
        {
            Coordinates = coordinates;
            Value = value;
        }
    }

    public class HeightMap
    {
        public Node DefaultStartNode { get; set; }
        public Node End { get; set; }
        public Node[,] Map { get; set; }

        public List<Node> GetOtherStartNodes(char? startValue)
        {
            var coordinates = new List<Node>();

            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int k = 0; k < Map.GetLength(1); k++)
                {
                    var node = Map[i, k];
                    if (node.Value == startValue)
                    {
                        coordinates.Add(node);
                    }
                }
            }

            return coordinates;
        }
    }

    public class Day12 : IPuzzle
    {
        public void Solve()
        {
            SolveFirst();
            SolveSecond();
        }

        private void SolveFirst()
        {
            var lines = File.ReadAllLines("inputs/day12.txt");
            var heightMap = CreateMap(lines);
            var node = ApplyBfs(heightMap, heightMap.DefaultStartNode);
            GetPathLength(node);
        }

        private void SolveSecond()
        {
            var lines = File.ReadAllLines("inputs/day12.txt");
            var heightMap = CreateMap(lines);
            var node = ApplyBfs(heightMap, heightMap.DefaultStartNode, true);
            GetPathLength(node);
        }

        private void GetPathLength(Node finalNode)
        {
            var length = 0;
            while (finalNode.Parent != null)
            {
                length++;
                finalNode = finalNode.Parent;
            }

            Console.WriteLine(length);
        }

        private Node ApplyBfs(HeightMap map, Node startNode, bool additionalStartNodes = false)
        {
            var queue = new Queue<Node>();
            startNode.Explored = true;
            queue.Enqueue(startNode);

            if (additionalStartNodes)
            {
                foreach (var node in map.GetOtherStartNodes('a'))
                {
                    node.Explored = true;
                    queue.Enqueue(node);
                }
            }

            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();

                if (vertex == map.End)
                {
                    return vertex;
                }

                var adjacentValues = GetAdjacentValues(map, vertex);
                foreach (var adjacent in adjacentValues)
                {
                    if (!adjacent.Explored)
                    {
                        if (vertex.Value + 1 >= adjacent.Value)
                        {
                            adjacent.Parent = vertex;
                            adjacent.Explored = true;
                            queue.Enqueue(adjacent);
                        }
                    }
                }
            }

            throw new Exception("There has been a big problem");
        }

        private List<Node> GetAdjacentValues(HeightMap map, Node node)
        {
            var adjacent = new List<Node>();

            var nodeMoves = new List<(int, int)>
            {
                (-1, 0), (1, 0), (0, -1), (0, 1)
            };

            foreach (var moves in nodeMoves)
            {
                var yCooridinate = node.Coordinates.Item1 + moves.Item1;
                var xCooridinate = node.Coordinates.Item2 + moves.Item2;
                if ((xCooridinate >= 0 && xCooridinate <= map.Map.GetLength(1) - 1) && (yCooridinate >= 0 && yCooridinate <= map.Map.GetLength(0) - 1))
                {
                    adjacent.Add(map.Map[yCooridinate, xCooridinate]);
                }
            }

            return adjacent;
        }

        private HeightMap CreateMap(string[] lines)
        {
            var heightMap = new HeightMap
            {
                Map = new Node[lines.Length, lines[0].Length]
            };

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                for (int c = 0; c < line.Length; c++)
                {
                    var value = line[c];
                    var node = new Node((i, c), value);

                    if (value == 'S')
                    {
                        node.Value = '~'; // make sure start is higher than anything else
                        heightMap.DefaultStartNode = node;
                    }
                    else if (value == 'E')
                    {
                        heightMap.End = node;
                    }
                    heightMap.Map[i, c] = node;
                }
            }

            return heightMap;
        }
    }
}
