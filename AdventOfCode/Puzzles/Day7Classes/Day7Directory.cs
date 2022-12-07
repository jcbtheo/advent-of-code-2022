using System.Collections.Generic;

namespace AdventOfCode.Puzzles.Day7Classes
{
    public class Day7Directory
    {
        public Day7Directory Parent { get; set; }
        public string Name { get; set; }
        public List<Day7File> Files { get; set; } = new List<Day7File>();
        public List<Day7Directory> Directories { get; set; } = new List<Day7Directory>();
    }

    public class Day7File
    {
        public int Size { get; set; }
        public string Name { get; set; }
    }
}
