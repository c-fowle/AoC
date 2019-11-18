using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Common;
using AoC.Common.Attributes;
using AoC.Common.ExtensionMethods;

namespace AoC._2017.Day05
{
    
    [Year(2017)]
    [Day(5)]
    //[Test("0\n3\n0\n1\n-3", "5", "10")]
    public class Year2017Day05 : Puzzle
    {
        private int[] ParseInput(string input) => input.Split('\n').ForEach(s => int.Parse(s)).ToArray();

        protected override string Part1(string input)
        {
            var parsedInput = ParseInput(input);
            var position = 0;
            var moveCount = 0;

            while (position < parsedInput.Length)
            {
                position = position + (parsedInput[position]++);
                ++moveCount;
            }

            return moveCount.ToString();
        }

        protected override string Part2(string input)
        {
            var parsedInput = ParseInput(input);
            var position = 0;
            var moveCount = 0;

            while (position < parsedInput.Length)
            {
                position = position + (parsedInput[position] >= 3 ? parsedInput[position]-- : parsedInput[position]++);
                ++moveCount;
            }

            return moveCount.ToString();
        }

        //=> throw new NotImplementedException("Part 2 not yet implemented...");
    }
}
