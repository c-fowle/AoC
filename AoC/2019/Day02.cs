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
    [Day(2)]
    public class Day02 : Puzzle
    {
        private IntcodeComputerFactory IntcodeComputerFactory { get; }
        private int[] ParseInput(string input) => input.Split(',').ForEach(s => int.Parse(s)).ToArray();

        public Day02() : base()
        {
            IntcodeComputerFactory = new IntcodeComputerFactory();
        }

        private IntcodeComputer GetIntcodeComputer(string input) => IntcodeComputerFactory.CreateIntcodeComputer(ParseInput(input), new int[] { 1, 2, 99 });

        protected override string Part1(string input)
        {
            var result = GetIntcodeComputer(input).RunProgram(0, new Action<int[]>(memory =>
            {
                memory[1] = 12;
                memory[2] = 2;
            }));

            if (!result.ExecutionSucceeded) throw new Exception("Intcode computer encountered an invalid operation code");

            return result.GetMemoryAddress(0).ToString();
        }

        protected override string Part2(string input)
        {      
            var noun = 0;
            var verb = 0;

            var computer = GetIntcodeComputer(input);
            var programInitialiser = new Action<int[]>(memory =>
            {
                memory[1] = noun;
                memory[2] = verb;
            });

            while (verb < 100)
            {
                var result = computer.RunProgram(0, programInitialiser);
                if (result.ExecutionSucceeded && result.GetMemoryAddress(0) == 19690720) return ((noun * 100) + verb).ToString();

                if (++noun >= 100)
                {
                    noun = 0;
                    ++verb;
                }
            }

            throw new Exception("Executed for all valid 'noun' and 'verb' values but no solution was found");
        }
    }
}
