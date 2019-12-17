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
    [Day(17)]
    public class Day17 : _2019Puzzle
    {
        protected override string Part1(string input)
        {
            var intcodeComputer = GetIntcodeComputer(input);
            //var i = ASCIIEncoding.ASCII.GetChars(new byte[] { 35 });

            var programResult = intcodeComputer.RunProgram(new IntcodeProgramInput()).GetAwaiter().GetResult();
            var screenState = new string(programResult.GetOutputs().SelectMany(i => ASCIIEncoding.ASCII.GetChars(new byte[] { (byte)i })).ToArray()).Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            var total = 0;

            for (var y = 1; y < screenState.Length - 1; ++y)
            {
                for (var x = 1; x < screenState[y].Length - 1; ++x)
                {
                    if (screenState[y][x] != '#') continue;
                    if (screenState[y][x - 1] == '#' && screenState[y][x + 1] == '#' && screenState[y - 1][x] == '#' && screenState[y + 1][x] == '#')
                    {
                        total += (y * x);
                    }
                }
            }

            return total.ToString();
        }

        protected override string Part2(string input)
        {
            throw new NotImplementedException();
        }
    }
}
