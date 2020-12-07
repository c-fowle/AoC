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
    [Day(23)]
    [Test("input",null, null)]
    [Test("input", null, null)]
    [Test("input", null, null)]
    [Test("input", null, null)]
    public class Day23 : _2019Puzzle
    {
        private int[] ParseInput(string input) => input.Split('\n').Select(s => int.Parse(s)).ToArray();

        protected override string Part1(string input)
        {
            while (true)
            {
                Console.WriteLine("Address:");
                var memAddress = long.Parse(Console.ReadLine());

                var computer = GetIntcodeComputer(input, defaultInput: -1);
                computer.RunProgram(new IntcodeProgramInput(inputs: new[] { memAddress }));

                while (computer.OutputReady || !computer.Paused)
                {
                    Console.WriteLine(computer.GetNextOutput());
                }

                Console.WriteLine("Complete");
                Console.ReadKey();
                Console.Clear();
            }
        }

        protected override string Part2(string input)
        {
            throw new NotImplementedException();
        }
    }
}
