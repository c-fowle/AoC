using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AoC.Common;
using AoC.Common.Attributes;
using AoC.Common.ExtensionMethods;

namespace AoC._2020
{
    [Year(2020)]
    [Day(5)]
    [Test("BFFFBBFRRR\nFFFBBBFRRR", "567", null)]
    [Test("BBFFBBFRLL\nFFFBBBFRRR", "820", null)]
    public class Day05: Puzzle
    {
        private List<int> ParseInput(string input)
        {
            var boardingPasses = input.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(s => new[] { s.Substring(0, 7), s.Substring(7, 3) }).ToArray();
            var numberOfSeats = new int[] { 128, 8 };
            var seatIds = new List<int>();

            boardingPasses.ForEach(boardingPass =>
            {
                var seatPosition = new int[2];

                for (var coordinate = 0; coordinate < 2; ++coordinate)
                {
                    var min = 0;
                    var max = numberOfSeats[coordinate] - 1;
                    boardingPass[coordinate].ForEach(instruction =>
                    {
                        if (instruction == 'F' || instruction == 'L') max = min + (int)Math.Floor((double)(max - min) / 2);
                        else if (instruction == 'B' || instruction == 'R') min = min + (int)Math.Ceiling((double)(max - min) / 2);
                        else throw new Exception("Unrecognised row instruction");
                    });

                    if (min != max) throw new Exception("Search went wrong");
                    seatPosition[coordinate] = max;
                }

                seatIds.Add((seatPosition[0] * 8) + seatPosition[1]);
            });

            return seatIds;
        }
        private List<int> ParseInputBinary(string input)
        {
            var brRegex = new Regex("[BR]");
            var flRegex = new Regex("[FL]");

            var binaryStrings = flRegex.Replace(brRegex.Replace(input, "1"), "0").Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return binaryStrings.Select(s => Convert.ToInt32(s, 2)).ToList();
        }

        protected override string Part1(string input) => ParseInputBinary(input).Max().ToString();
        protected override string Part2(string input)
        {
            var seatIds = ParseInputBinary(input);
            var mySeatIdNeighbour = seatIds.Single(s => seatIds.Contains(s + 2) && !seatIds.Contains(s + 1));
            return (mySeatIdNeighbour + 1).ToString();
        }
    }
}

