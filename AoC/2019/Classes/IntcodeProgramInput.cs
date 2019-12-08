using System;

namespace AoC._2019.Classes
{
    public class IntcodeProgramInput
    {
        public Action<int[]> MemoryInitialisation { get; }
        public int[] Inputs { get; }
        public InputMode InputMode { get; }

        public IntcodeProgramInput(Action<int[]> memoryInitialisation = null, int[] inputs = null, InputMode inputMode = InputMode.UseOnce)
        {
            MemoryInitialisation = memoryInitialisation ?? new Action<int[]>(mem => { return; });

            Inputs = new int[inputs == null ? 0 : inputs.Length];
            if (inputs != null && inputs.Length > 0) inputs.CopyTo(Inputs, 0);

            InputMode = inputMode;
        }
    }
}
