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

namespace AoC._2020
{
    [Year(2020)]
    [Day(1)]
    [Test("1721\n979\n366\n299\n675\n1453", "514579", "241861950")]
    public class Day01 : Puzzle
    {
        private int[] ParseInput(string input) => input.Split('\n').Select(s => int.Parse(s)).ToArray();

        protected override string Part1(string input)
        {
            var parsedInput = ParseInput(input);

            for (var i = 0; i < parsedInput.Length; ++i)
            {
                for (var j = (i + 1); j < parsedInput.Length; ++j)
                {
                    if (parsedInput[i] + parsedInput[j] == 2020)
                    {
                        return (parsedInput[i] * parsedInput[j]).ToString();
                    }
                }
            }

            throw new Exception("Could not find pair which sum to 2020");
        }

        protected override string Part2(string input)
        {
            var parsedInput = ParseInput(input);

            for (var i = 0; i < parsedInput.Length; ++i)
            {
                for (var j = (i + 1); j < parsedInput.Length; ++j)
                {
                    for (var k = (j + 1); k < parsedInput.Length; ++k)
                    {
                        if (parsedInput[i] + parsedInput[j] + parsedInput[k] == 2020)
                        {
                            return (parsedInput[i] * parsedInput[j] * parsedInput[k]).ToString();
                        }
                    }
                }
            }

            throw new Exception("Could not find pair which sum to 2020");
        }
    }
}
