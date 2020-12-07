using System;
using System.Linq;

using AoC.Common;

using AoC._2019.Classes;
using AoC._2019.Enums;

namespace AoC._2019
{
    public abstract class _2019Puzzle : Puzzle
    {
        private IntcodeComputerFactory IntcodeComputerFactory { get; }

        public _2019Puzzle() : base()
        {
            IntcodeComputerFactory = new IntcodeComputerFactory();
        }

        protected IntcodeComputer GetIntcodeComputer(string input, int[] allowedOperations = null, long? defaultInput = null)
        {
            var parsedInput = input.Split(',').Select(s => long.Parse(s.Trim())).ToArray();
            return IntcodeComputerFactory.CreateIntcodeComputer(parsedInput, allowedOperations, defaultInput);
        }
    }
}
 