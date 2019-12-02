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
        private int[] ParseInputToInt(string input) => input.Split(',').ForEach(s => int.Parse(s)).ToArray();
        private IntcodeComputer CreateIntcodeComputer(int[] initialMemoryState)
        {
            var opcodeAdd = new Opcode(1, 3, new Func<int, int[], bool>((pointer, memory) =>
            {
                memory[memory[pointer + 3]] = memory[memory[pointer + 1]] + memory[memory[pointer + 2]];
                return false;
            }));
            var opcodeMultiply = new Opcode(2, 3, new Func<int, int[], bool>((pointer, memory) =>
            {
                memory[memory[pointer + 3]] = memory[memory[pointer + 1]] * memory[memory[pointer + 2]];
                return false;
            }));
            var opcodeExit = new Opcode(99, 0, new Func<int, int[], bool>((pointer, memory) => true));

            return new IntcodeComputer(new List<Opcode> { opcodeAdd, opcodeMultiply, opcodeExit }, initialMemoryState);
        }

        protected override string Part1(string input)
        {
            var finalState = CreateIntcodeComputer(ParseInputToInt(input)).RunProgram(new Action<int[]>(memory =>
            {
                memory[1] = 12;
                memory[2] = 2;
            }));

            if (finalState is null) throw new Exception("Intcode computer encountered an invalid operation code");

            return finalState[0].ToString();
        }

        protected override string Part2(string input)
        {      
            var noun = 0;
            var verb = 0;

            var computer = CreateIntcodeComputer(ParseInputToInt(input));
            var programInitialiser = new Action<int[]>(memory =>
            {
                memory[1] = noun;
                memory[2] = verb;
            });

            while (verb < 100)
            {
                var finalState = computer.RunProgram(programInitialiser);
                if (finalState != null && finalState[0] == 19690720) return ((noun * 100) + verb).ToString();

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
