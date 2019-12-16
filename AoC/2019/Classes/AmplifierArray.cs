using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AoC.Common.ExtensionMethods;

namespace AoC._2019.Classes
{
    public class AmplifierArray
    {
        private List<IntcodeComputer> Amplifiers { get; }
        private List<long> Phases { get; }
        private bool Completed { get; set; }
        private long ProgramOutput { get; set; }
        public AmplifierArray(Func<IntcodeComputer> createIntcodeComputer, List<long> phases)
        {
            Amplifiers = new List<IntcodeComputer>();
            phases.ForEach(phase => Amplifiers.Add(createIntcodeComputer()));

            Phases = phases.CloneAsList().ToList();
        }

        public async Task<long> RunProgram()
        {
            if (Completed) return ProgramOutput;

            for (var ampCount = 0; ampCount < Amplifiers.Count; ++ampCount) Amplifiers[ampCount].RunProgram(new IntcodeProgramInput(inputs: new[] { Phases[ampCount] }));

            Amplifiers.First().AddInput(0);

            while (!Amplifiers.Last().Exited && Amplifiers.All(amp => !amp.Errored))
            {
                for(var ampCount = 0; ampCount < (Amplifiers.Count); ++ampCount)
                {
                    var ampOutput = Amplifiers[ampCount].GetNextOutput();
                    if (!ampOutput.HasValue || Amplifiers[(ampCount + 1) % Amplifiers.Count].Exited) continue;
                    Amplifiers[(ampCount + 1) % Amplifiers.Count].AddInput(ampOutput.Value);
                }
            }

            ProgramOutput = Amplifiers.Last().GetNextOutput().Value;
            Completed = true;

            return ProgramOutput;
        }
    }
}

