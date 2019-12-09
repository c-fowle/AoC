using System;

namespace AoC._2019.Classes
{
    public class IntcodeProgramInput
    {
        public Action<long[]> MemoryInitialisation { get; }
        public long[] Inputs { get; }

        public IntcodeProgramInput(Action<long[]> memoryInitialisation = null, long[] inputs = null)
        {
            MemoryInitialisation = memoryInitialisation ?? new Action<long[]>(mem => { return; });

            Inputs = new long[inputs == null ? 0 : inputs.Length];
            if (inputs != null && inputs.Length > 0) inputs.CopyTo(Inputs, 0);
        }
    }
}
