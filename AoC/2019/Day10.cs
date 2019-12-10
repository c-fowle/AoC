using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Common;
using AoC.Common.Attributes;
using AoC.Common.ExtensionMethods;

using AoC._2019.Classes;
using AoC._2019.Enums;

namespace AoC._2019
{

    [Year(2019)]
    [Day(10)]
    [Test(".#..#\n.....\n#####\n....#\n...##","8", null)]
    [Test("......#.#.\n#..#.#....\n..#######.\n.#.#.###..\n.#..#.....\n..#....#.#\n#..#....#.\n.##.#..###\n##...#..#.\n.#....####", "33", null)]
    [Test("#.#...#.#.\n.###....#.\n.#....#...\n##.#.#.#.#\n....#.#.#.\n.##..###.#\n..#...##..\n..##....##\n......#...\n.####.###.", "35", null)]
    [Test(".#..##.###...#######\n##.############..##.\n.#.######.########.#\n.###.#######.####.#.\n#####.##.#.##.###.##\n..#####..#.#########\n####################\n#.####....###.#.#.##\n##.#################\n#####.##.###..####..\n..######..##.#######\n####.##.####...##..#\n.#####..#.######.###\n##...#.##########...\n#.##########.#######\n.####.#.###.###.#.##\n....##.##.###..#####\n.#.#.###########.###\n#.#.#.#####.####.###\n###.##.####.##.#..##", "210", "802")]
    public class Day10 : _2019Puzzle
    {
        private bool[][] ParseInput(string input) => input.Split('\n').Select(s => s.Select(c => c =='#').ToArray()).ToArray();

        private Quadrant GetQuadrant(int x, int y)
        {
            if (x >= 0 && y > 0) return Quadrant.TopRight;
            else if (x > 0 && y <= 0) return Quadrant.BottomRight;
            else if (x <= 0 && y < 0) return Quadrant.BottomLeft;
            return Quadrant.TopLeft;
        }
        private List<int?> GetFactors(int i)
        {
            var factors = new List<int?>();
            for (var factor = 1; factor <= Math.Sqrt(i); ++factor) if ((i % factor) == 0) factors.Add(factor);
            return factors;
        }
        private int? GetLargestCommonFactor(int a, int b)
        {
            var modA = Math.Abs(a);
            var modB = Math.Abs(b);

            if (modA == 0 && modB == 0) return null;
            if (modA == 0 && modB != 0) return modB;
            if (modA == 0 && modB == 0) return modA;
            if (modA == modB) return modA;

            var bFactors = GetFactors(b);
            return GetFactors(a).Where(f => bFactors.Contains(f)).OrderByDescending(i => i).FirstOrDefault();
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
        private List<Tuple<decimal, int[]>> GetVisibleAsteroids (bool[][] asteroidMap, int x, int y)
        {
            var visibleLocations = new List<Tuple<decimal, int[]>>();

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

                        var thisAngle = GetAngle(xDiff, (-yDiff));
                        if (visibleLocations.All(i => i.Item1 != thisAngle)) visibleLocations.Add(new Tuple<decimal, int[]>(thisAngle, new int[] { x + xDiff, y + yDiff }));
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
            var visibleAsteroids = default(List<Tuple<decimal, int[]>>);

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
                visibleAsteroids = visibleAsteroids.OrderBy(i => i.Item1).ToList();

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
