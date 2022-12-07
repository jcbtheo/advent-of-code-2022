using AdventOfCode.Puzzles.Day7Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Puzzles
{
    public class Day7 : IPuzzle
    {
        private enum Commands
        {
            ChangeDirectory,
            List,
            Directory,
            File
        }

        public void Solve()
        {
            var lines = File.ReadAllLines("inputs/day7.txt");

            var fileSystem = new Day7Directory
            {
                Name = "/"
            };
            var current = fileSystem;

            foreach (var line in lines)
            {
                var command = GetCommandType(line);
                current = ApplyCommand(current, command, line);
            }

            var totals = new List<int>();
            var totalRootSize = GetDirectorySize(fileSystem, totals);

            var availableDiskSpace = 70000000;
            var requiredFreeSpace = 30000000;
            var currentFreeSpace = availableDiskSpace - totalRootSize;

            var sum = totals
                .Where(x => x <= 100000)
                .Sum(x => x);

            Console.WriteLine($"Sum of directories having size of no more than 100000: {sum}");

            var smallest = totals
                .Where(x => currentFreeSpace + x >= requiredFreeSpace)
                .OrderBy(x => x)
                .First();

            Console.WriteLine($"Smallest directory size to free space: {smallest}");
        }

        private int GetDirectorySize(Day7Directory directory, List<int> totals)
        {
            var size = 0;

            size += directory.Files.Sum(x => x.Size);
            size += directory.Directories.Sum(x => GetDirectorySize(x, totals));

            //Console.WriteLine($"{directory.Name} Size: {size}");
            totals.Add(size);

            return size;
        }

        private Day7Directory ApplyCommand(Day7Directory currentDirectory, Commands command, string line)
        {
            switch (command)
            {
                case Commands.Directory:
                    currentDirectory.Directories.Add(BuildDirectory(line, currentDirectory));
                    break;
                case Commands.File:
                    currentDirectory.Files.Add(BuildFile(line));
                    break;
                case Commands.ChangeDirectory:
                    var directoryValue = GetChangeDirectoryValue(line);
                    if(directoryValue == "/")
                    {
                        while (currentDirectory.Parent != null)
                        {
                            currentDirectory = currentDirectory.Parent;
                        }
                    }
                    else if (directoryValue == "..")
                    {
                        currentDirectory = currentDirectory.Parent ?? currentDirectory;
                    }
                    else
                    {
                        currentDirectory = currentDirectory.Directories.Where(x => x.Name == directoryValue).First();
                    }
                    break;
                case Commands.List:
                    break;
            }

            return currentDirectory;
        }

        private Day7Directory BuildDirectory(string line, Day7Directory parent)
        {
            var values = line.Split(" ");
            return new Day7Directory
            {
                Name = values[1],
                Parent = parent
            };
        }

        private Day7File BuildFile(string line)
        {
            var values = line.Split(" ");
            return new Day7File
            {
                Size = int.Parse(values[0]),
                Name = values[1]
            };
        }

        private string GetChangeDirectoryValue(string line)
        {
            var values = line.Split(" ");
            return values[2];
        }

        private Commands GetCommandType(string line)
        {
            if (line.StartsWith("$"))
            {
                if (line[2] == 'c')
                {
                    return Commands.ChangeDirectory;
                }

                return Commands.List;
            }

            if (line.StartsWith("dir"))
            {
                return Commands.Directory;
            }

            return Commands.File;
        }
    }
}
