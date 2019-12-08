using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using AoC.Common;
using AoC.Common.Attributes;
using AoC.Common.ExtensionMethods;

using AoC._2019.Classes;

namespace AoC._2019
{
    [Year(2019)]
    [Day(7)]
    [Test("3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0", "43210", null)]
    [Test("3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0", "54321", null)]
    [Test("3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0", "65210", null)]
    [Test("3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5", null, "139629729")]
    [Test("3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,-5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10", null, "18216")]
    public class Day07 : Puzzle
    {
        private IntcodeComputerFactory IntcodeComputerFactory { get; }
        public Day07() : base()
        {
            IntcodeComputerFactory = new IntcodeComputerFactory();
        }

        private int[] ParseInput(string input) => input.Split(',').ForEach(s => int.Parse(s)).ToArray();

        private IntcodeComputer GetIntcodeComputer(string input) => IntcodeComputerFactory.CreateIntcodeComputer(ParseInput(input));

        private List<List<int>> GetPermutations(List<int> list)
        {
            if (list.Count == 1) return new List<List<int>>() { new List<int>() { list.Single() } };

            var permutations = new List<List<int>>();

            for (var pos = 0; pos < list.Count; ++pos)
            {
                var listCopy = list.CloneAsList().ToList();
                var initial = listCopy[pos];
                listCopy.RemoveAt(pos);

                var sublistPermutations = GetPermutations(listCopy);

                sublistPermutations.ForEach(l =>
                {
                    l.Insert(0, initial);
                    permutations.Add(l.CloneAsList().ToList());
                });
            }

            return permutations;
        }

        protected override string Part1(string input)
        {
            var intcodeComputer = GetIntcodeComputer(input);
            var possibleSequences = GetPermutations(new List<int>() { 0, 1, 2, 3, 4 });

            var maxThrust = 0;

            possibleSequences.ForEach(seq =>
            {
                var programInput = 0;
                var errorEncountered = false;

                seq.ForEach(phaseSet =>
                {
                    if (errorEncountered) return;

                    var programResult = intcodeComputer.RunProgram(new IntcodeProgramInput(inputs: new int[] { phaseSet, programInput })).GetAwaiter().GetResult();

                    if (!programResult.ExecutionSucceeded || !programResult.LastOutput.HasValue) errorEncountered = true;
                    else programInput = programResult.LastOutput.Value;
                });

                if (!errorEncountered && programInput > maxThrust) maxThrust = programInput;
            });

            return maxThrust.ToString();
        }

        protected override string Part2(string input)
        {
            var intcodeComputer = GetIntcodeComputer(input);
            var possibleSequences = GetPermutations(new List<int>() { 5, 6, 7, 8, 9 });

            var maxThrust = 0;

            for (var seqCount = 0; seqCount < possibleSequences.Count; ++seqCount)
            {
                var seq = possibleSequences[seqCount];

                var amplifiers = new List<IntcodeComputer>
                {
                    GetIntcodeComputer(input),
                    GetIntcodeComputer(input),
                    GetIntcodeComputer(input),
                    GetIntcodeComputer(input),
                    GetIntcodeComputer(input)
                };

                for (var ampCount = 0; ampCount < amplifiers.Count; ++ampCount) amplifiers[ampCount].RunProgram(new IntcodeProgramInput(inputs: new[] { seq[ampCount] }));

                amplifiers[0].AddInput(0);

                while (!amplifiers.Last().Exited && !amplifiers.Any(amp => amp.Errored))
                {
                    for (var ampCount = 0; ampCount < amplifiers.Count; ++ampCount)
                    {
                        if (amplifiers[ampCount].OutputReady())
                        {
                            if (ampCount == amplifiers.Count - 1 && amplifiers[ampCount].Exited) break;
                            amplifiers[(ampCount + 1) % amplifiers.Count].AddInput(amplifiers[ampCount].GetLastOutput().Value);
                        }
                    }
                }

                if (!amplifiers.Last().Errored)
                {
                    var finalOutput = 0;
                    while (amplifiers.Last().OutputReady()) finalOutput = amplifiers.Last().GetLastOutput().Value;

                    if (finalOutput > maxThrust) maxThrust = finalOutput;
                }

                amplifiers.ForEach(amp => amp.ForceStop());
            }

            return maxThrust.ToString();
        }
    }
}
