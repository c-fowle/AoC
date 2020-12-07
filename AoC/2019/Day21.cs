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
    [Day(21)]
    public class Day21 : _2019Puzzle
    {
        private string ExecuteSpringdroidProgram(string input, string springscript)
        {
            var intcodeComputer = GetIntcodeComputer(input);

            var encodedProgram = ASCIIEncoding.ASCII.GetBytes(springscript);
            var inputList = encodedProgram.Select(b => (long)b).ToArray();

            var programResult = intcodeComputer.RunProgram(new IntcodeProgramInput(inputs: inputList)).GetAwaiter().GetResult();
            var programOutputs = programResult.GetOutputs();

            if (programOutputs.Last() > 127) return programOutputs.Last().ToString();

            Console.WriteLine("Program failed...");
            Console.WriteLine();

            var endingState = new string(ASCIIEncoding.ASCII.GetChars(programOutputs.Select(l => (byte)l).ToArray()));

            Console.WriteLine(endingState);
            Console.ReadLine();

            throw new Exception("Springdroid fell into a hole");
        }

        protected override string Part1(string input) => ExecuteSpringdroidProgram(input, "OR A J\nAND B J\nAND C J\nNOT J J\nAND D J\nWALK\n");

        protected override string Part2(string input) => ExecuteSpringdroidProgram(input, "OR A J\nAND B J\nAND C J\nNOT J J\nAND D J\nOR F T\nOR I T\nAND E T\nOR H T\nAND T J\nRUN\n");
    }
}
