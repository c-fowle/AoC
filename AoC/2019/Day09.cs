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
    [Day(9)]
    [Test("109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99", "99", null)]
    [Test("104,1125899906842624,99", "1125899906842624", null)]
    public class Day09 : _2019Puzzle
    {
        protected override string Part1(string input) => GetIntcodeComputer(input).RunProgram(new IntcodeProgramInput(inputs: new[] { 1L })).GetAwaiter().GetResult().LastOutput?.ToString();
        protected override string Part2(string input) => GetIntcodeComputer(input).RunProgram(new IntcodeProgramInput(inputs: new[] { 2L })).GetAwaiter().GetResult().LastOutput?.ToString();
    }
}
