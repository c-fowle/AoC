using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Common;
using AoC.Common.Attributes;
using AoC.Common.ExtensionMethods;

namespace AoC._2020
{
    [Year(2020)]
    [Day(3)]
    public class Day03 : Puzzle
    {
        private int[][] ParseInput(string input) => input.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Select(c => c == '.' ? 0 : 1).ToArray()).ToArray();

        private int GetTreesByRoute(int[][] map, int[] route)
        {
            var positionX = 0;
            var positionY = 0;
            var trees = 0;

            while (positionY < map.GetLength(0))
            {
                if (map[positionY][positionX] == 1) trees += 1;
                positionX = ((positionX + route[0]) % map[0].Length);
                positionY += route[1];
            }

            return trees;
        }

        protected override string Part1(string input) => GetTreesByRoute(ParseInput(input), new[] { 3, 1 }).ToString();
        protected override string Part2(string input)
        {
            var map = ParseInput(input);
            var routes = new List<int[]> { new[] { 1, 1 }, new[] { 3, 1 }, new[] { 5, 1 }, new[] { 7, 1 }, new[] { 1, 2 } };
            return routes.Select(r => GetTreesByRoute(map, r)).ToArray().Product().ToString();
        }
    }
}
