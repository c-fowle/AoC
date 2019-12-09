using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private long[] ParseInput(string input) => input.Split(',').ForEach(s => long.Parse(s)).ToArray();

        public Day02() : base()
        {
            IntcodeComputerFactory = new IntcodeComputerFactory();
        }

        private IntcodeComputer GetIntcodeComputer(string input) => IntcodeComputerFactory.CreateIntcodeComputer(ParseInput(input), new int[] { 1, 2, 99 });

        protected override string Part1(string input)
        {
            var result = GetIntcodeComputer(input).RunProgram(new IntcodeProgramInput(new Action<long[]>(memory =>
            {
                memory[1] = 12;
                memory[2] = 2;
            }))).GetAwaiter().GetResult();

            if (!result.ExecutionSucceeded) throw new Exception("Intcode computer encountered an invalid operation code");

            return result.GetMemoryAddress(0).ToString();
        }

        protected override string Part2(string input)
        {      
            var noun = 0;
            var verb = 0;
            var solution = default(string);

            var programInitialiser = new Action<long[]>(memory =>
            {
                memory[1] = noun;
                memory[2] = verb;
            });
            var programInput = new IntcodeProgramInput(memoryInitialisation: programInitialiser);
            var allComputers = new Dictionary<string, IntcodeComputer>();

            while(verb < 100)
            {
                var thisKey = ((noun * 100) + verb).ToString();
                allComputers = allComputers.Where(kvp => !kvp.Value.Exited || (kvp.Value.Exited && kvp.Value.CurrentMemory[0] == 19690720)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                allComputers.Add(thisKey, GetIntcodeComputer(input));

                var programTask = allComputers[thisKey].RunProgram(programInput);
                var completed = allComputers.Where(kvp => kvp.Value.Exited && kvp.Value.CurrentMemory[0] == 19690720);

                if (completed.Count() > 0) return completed.First().Key;

                if (++noun >= 100)
                {
                    noun = 0;
                    ++verb;
                }
            }

            do
            {
                allComputers.ForEach(kvp =>
                {
                    if (solution == null && kvp.Value.Exited && kvp.Value.CurrentMemory[0] == 19690720) solution = kvp.Key;
                });

                if (solution != null) return solution;

            } while (!allComputers.All(kvp => kvp.Value.Exited));

            throw new Exception("Executed for all valid 'noun' and 'verb' values but no solution was found");
        }
    }
}
 