using System.Collections.Generic;
using AoC.Common.ExtensionMethods;

namespace AoC._2019.Classes
{
    public class IntcodeProgramResult
    {
        public bool ExecutionSucceeded { get; }
        private int[] FinalMemoryState { get; }
        private IList<int> Outputs { get; }
        public IntcodeProgramResult(bool success, int[] finalMemoryState, List<int> outputs)
        {
            ExecutionSucceeded = success;

            FinalMemoryState = new int[finalMemoryState.Length];
            finalMemoryState.CopyTo(FinalMemoryState, 0);

            Outputs = new List<int>();
            outputs.ForEach(Outputs.Add);
        }

        public int GetMemoryAddress(int address) => FinalMemoryState[address];
        public IList<int> GetOutputs()
        {
            var outputCopy = new List<int>();
            Outputs.ForEach(outputCopy.Add);
            return outputCopy;
        }
        public int? LastOutput => Outputs.Count > 0 ? (int?)Outputs[Outputs.Count - 1] : null;
    }
}
