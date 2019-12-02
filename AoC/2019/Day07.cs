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
    [Year(2019)]
    [Day(7)]
    [Test("input",null, null)]
    [Test("input", null, null)]
    [Test("input", null, null)]
    [Test("input", null, null)]
    public class Day07 : Puzzle
    {
        private int[] ParseInput(string input) => input.Split('\n').ForEach(s => int.Parse(s)).ToArray();

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
