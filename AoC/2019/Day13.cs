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
    [Day(13)]
    [Test("input",null, null)]
    [Test("input", null, null)]
    [Test("input", null, null)]
    [Test("input", null, null)]
    public class Day13 : _2019Puzzle
    {
        private int[] ParseInput(string input) => input.Split('\n').Select(s => int.Parse(s)).ToArray();

        protected override string Part1(string input)
        {
            throw new NotImplementedException();
        }

        protected override string Part2(string input)
        {
            throw new NotImplementedException();
        }
    }
}
