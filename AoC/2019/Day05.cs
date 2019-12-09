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
    [Day(5)]
    public class Day05 : Puzzle
    {
        private IntcodeComputerFactory IntcodeComputerFactory { get; }
        private long[] ParseInput(string input) => input.Split(',').ForEach(s => long.Parse(s)).ToArray();
        public Day05() : base()
        {
            IntcodeComputerFactory = new IntcodeComputerFactory();
        }

        private IntcodeComputer GetIntcodeComputer(string input) => IntcodeComputerFactory.CreateIntcodeComputer(ParseInput(input));
        protected override string Part1(string input) => GetIntcodeComputer(input).RunProgram(new IntcodeProgramInput(inputs: new[] { 1L })).GetAwaiter().GetResult().LastOutput?.ToString();
        protected override string Part2(string input) => GetIntcodeComputer(input).RunProgram(new IntcodeProgramInput(inputs: new[] { 5L })).GetAwaiter().GetResult().LastOutput?.ToString();
    }
}
