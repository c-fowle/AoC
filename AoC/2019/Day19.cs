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
    [Day(19)]
    public class Day19 : _2019Puzzle
    {
        protected override string Part1(string input)
        {
            var affectedTileCount = 0;

            for (var x = 0; x < 50; ++x)
            {
                for (var y = 0; y < 50; ++y)
                {
                    var intcodeComputer = GetIntcodeComputer(input);
                    var tileResult = intcodeComputer.RunProgram(new IntcodeProgramInput(inputs: new long[] { (long)x, (long)y })).GetAwaiter().GetResult();

                    affectedTileCount += (int)tileResult.LastOutput;
                }
            }

            return affectedTileCount.ToString();
        }

        protected override string Part2(string input)
        {
            var tiles = new Dictionary<string, bool>();
            var distance = 0;

            while (true)
            {
                for (var i = 0; i <= distance; ++i)
                {
                    // x = i; y = distance
                    var tileKey = i.ToString() + "-" + distance.ToString();
                    

                    // x = distance; y = i
                }

                distance++;
            }
        }
    }
}
