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
    [Day(1)]
    [Test("12","2", null)]
    [Test("14", "2", "2")]
    [Test("1969", "654", "966")]
    [Test("100756", "33583", "50346")]
    [Test("12\n14\n1969\n100756", "34241", "51316")]
    public class Day01 : _2019Puzzle
    {
        private int[] ParseInput(string input) => input.Split('\n').Select(s => int.Parse(s)).ToArray();

        protected override string Part1(string input) => ParseInput(input).Select(i => (int)Math.Floor((double)i / 3d) - 2).Sum().ToString();

        protected override string Part2(string input)
        {
            var parsedInput = ParseInput(input);

            var fuelRequirement = parsedInput.Select(i =>
            {
                var totalFuel = 0;
                var thisMass = i;

                while (true)
                {
                    var thisFuel = (int)Math.Floor((double)thisMass / 3d) - 2;
                    if (thisFuel < 0) break;
                    thisMass = thisFuel;
                    totalFuel += thisFuel;
                }

                return totalFuel;
            });

            return fuelRequirement.Sum().ToString();
        }
    }
}
