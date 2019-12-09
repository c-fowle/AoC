using System.Collections.Generic;
using AoC.Common.ExtensionMethods;

namespace AoC._2019.Classes
{
    public class IntcodeProgramResult
    {
        public bool ExecutionSucceeded { get; }
        private long[] FinalMemoryState { get; }
        private IList<long> Outputs { get; }
        public IntcodeProgramResult(bool success, long[] finalMemoryState, List<long> outputs)
        {
            ExecutionSucceeded = success;

            FinalMemoryState = new long[finalMemoryState.Length];
            finalMemoryState.CopyTo(FinalMemoryState, 0);

            Outputs = new List<long>();
            outputs.ForEach(Outputs.Add);
        }

        public long GetMemoryAddress(ulong address) => FinalMemoryState[address];
        public IList<long> GetOutputs()
        {
            var outputCopy = new List<long>();
            Outputs.ForEach(outputCopy.Add);
            return outputCopy;
        }
        public long? LastOutput => Outputs.Count > 0 ? (long?)Outputs[Outputs.Count - 1] : null;
    }
}
