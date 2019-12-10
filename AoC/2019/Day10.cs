using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Common;
using AoC.Common.Attributes;
using AoC.Common.ExtensionMethods;

using AoC._2019.Classes;

namespace AoC._2019
{
    public enum Quadrant
    {
        TopRight = 0,
        BottomRight,
        BottomLeft,
        TopLeft
    }

    [Year(2019)]
    [Day(10)]
    [Test(".#..#\n.....\n#####\n....#\n...##","8", null)]
    [Test("......#.#.\n#..#.#....\n..#######.\n.#.#.###..\n.#..#.....\n..#....#.#\n#..#....#.\n.##.#..###\n##...#..#.\n.#....####", "33", null)]
    [Test("#.#...#.#.\n.###....#.\n.#....#...\n##.#.#.#.#\n....#.#.#.\n.##..###.#\n..#...##..\n..##....##\n......#...\n.####.###.", "35", null)]
    [Test(".#..##.###...#######\n##.############..##.\n.#.######.########.#\n.###.#######.####.#.\n#####.##.#.##.###.##\n..#####..#.#########\n####################\n#.####....###.#.#.##\n##.#################\n#####.##.###..####..\n..######..##.#######\n####.##.####...##..#\n.#####..#.######.###\n##...#.##########...\n#.##########.#######\n.####.#.###.###.#.##\n....##.##.###..#####\n.#.#.###########.###\n#.#.#.#####.####.###\n###.##.####.##.#..##", "210", "802")]
    public class Day10 : Puzzle
    {
        private bool[][] ParseInput(string input) => input.Split('\n').ForEach(s => s.Select(c => c =='#').ToArray()).ToArray();

        private Quadrant GetQuadrant(int x, int y)
        {
            if (x >= 0 && y > 0) return Quadrant.TopRight;
            else if (x > 0 && y <= 0) return Quadrant.BottomRight;
            else if (x <= 0 && y < 0) return Quadrant.BottomLeft;
            return Quadrant.TopLeft;
        }
        private int? GetLargestCommonFactor(int a, int b)
        {
            if (a == 0 && b == 0) return null;
            if (a == 0 && b != 0) return Math.Abs(b);
            if (a != 0 && b == 0) return Math.Abs(a);

            var modA = Math.Abs(a);
            var modB = Math.Abs(b);

            if (modA == modB) return modA;

            var aFactors = new List<int>();

            for (var d = 1; d <= Math.Sqrt(modA); ++d)
            {
                if ((modA % d) == 0)
                {
                    aFactors.Add(d);
                    aFactors.Add(modA / d);
                }
            }

            var bFactors = new List<int>();

            for (var d = 1; d <= Math.Sqrt(modB); ++d)
            {
                if ((modB % d) == 0)
                {
                    bFactors.Add(d);
                    bFactors.Add(modB / d);
                }
            }

            var common = aFactors.Where(f => bFactors.Contains(f));

            if (common.Count() == 0) return null;

            return common.OrderByDescending(i => i).First();
        }

        private int[] GetRatio(int x, int y)
        {
            var highestCommonFactor = GetLargestCommonFactor(x, y);
            if (!highestCommonFactor.HasValue) return new int[] { x, y };
            return new int[] { x / highestCommonFactor.Value, y / highestCommonFactor.Value };
        }

        private decimal GetAngle(params int[] coords)
        {
            if (coords[1] == 0)
            {
                if (coords[0] > 0) return (decimal)Math.PI / 2;
                else return (3 * (decimal)Math.PI) / 2;
            }

            var baseAngle = (decimal)Math.Atan((double)Math.Abs(coords[0]) / (double)Math.Abs(coords[1]));

            switch (GetQuadrant(coords[0], coords[1]))
            {
                case Quadrant.TopRight:
                    return baseAngle;
                case Quadrant.BottomRight:
                    return (((decimal)Math.PI / 2M) - baseAngle) + ((decimal)Math.PI / 2M);
                case Quadrant.BottomLeft:
                    return baseAngle + (decimal)Math.PI;
                case Quadrant.TopLeft:
                    return (((decimal)Math.PI / 2M) - baseAngle) + (3M * ((decimal)Math.PI / 2M));
            }

            throw new Exception();
        }
        private List<Tuple<int[], int[]>> GetVisibleAsteroids (bool[][] asteroidMap, int x, int y)
        {
            var visibleLocations = new List<Tuple<int[], int[]>>();

            for (var shell = 1; shell <= asteroidMap.Length; ++shell)
            {
                for (var xDiff = -shell; xDiff <= shell; ++xDiff)
                {
                    for (var yDiff = -shell; yDiff <= shell; ++yDiff)
                    {
                        if (Math.Abs(xDiff) != shell && Math.Abs(yDiff) != shell) continue;
                        if (xDiff == 0 && yDiff == 0) continue;
                        if ((y + yDiff) < 0 || (y + yDiff) >= asteroidMap.Length) continue;
                        if ((x + xDiff) < 0 || (x + xDiff) >= asteroidMap.Length) break;
                        if (!asteroidMap[y + yDiff][x + xDiff]) continue;

                        var thisRatio = GetRatio(xDiff, yDiff);
                        if (!visibleLocations.Any(i => i.Item1[0] == thisRatio[0] && i.Item1[1] == thisRatio[1])) visibleLocations.Add(new Tuple<int[], int[]>(thisRatio, new int[] { x + xDiff, y + yDiff }));
                    }
                }
            }

            return visibleLocations;
        }
        protected override string Part1(string input)
        {
            var asteroidMap = ParseInput(input);
            var maxVisible = 0;

            for(var y = 0; y < asteroidMap.Length; ++y)
            {
                for (var x = 0; x < asteroidMap[y].Length; ++x)
                {
                    if (!asteroidMap[y][x]) continue;
                    var visible = GetVisibleAsteroids(asteroidMap, x, y).Count;
                    if (visible > maxVisible) maxVisible = visible;
                }
            }

            return maxVisible.ToString();
        }

        protected override string Part2(string input)
        {
            var asteroidMap = ParseInput(input);
            var visibleAsteroids = default(List<Tuple<int[], int[]>>);

            var laserX = 0;
            var laserY = 0;

            for (var y = 0; y < asteroidMap.Length; ++y)
            {
                for (var x = 0; x < asteroidMap[y].Length; ++x)
                {
                    if (!asteroidMap[y][x]) continue;
                    var visible = GetVisibleAsteroids(asteroidMap, x, y);
                    if (visible.Count > (visibleAsteroids?.Count ?? 0))
                    {
                        visibleAsteroids = visible.CloneAsList().ToList();
                        laserX = x;
                        laserY = y;
                    }
                }
            }

            var asteroidsDestroyed = 0;

            while (visibleAsteroids.Count > 0)
            {
                visibleAsteroids = visibleAsteroids.OrderBy(i => GetAngle(i.Item2[0] - laserX, laserY - i.Item2[1])).ToList();

                for (var count = 0; count < visibleAsteroids.Count; ++count)
                {
                    asteroidMap[visibleAsteroids[count].Item2[0]][visibleAsteroids[count].Item2[1]] = false;
                    ++asteroidsDestroyed;

                    if (asteroidsDestroyed == 200) return ((visibleAsteroids[count].Item2[0] * 100) + visibleAsteroids[count].Item2[1]).ToString();
                }

                visibleAsteroids = GetVisibleAsteroids(asteroidMap, laserX, laserY);
            }

            throw new Exception();
        }
    }
}
