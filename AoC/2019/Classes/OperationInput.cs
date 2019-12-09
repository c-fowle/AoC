using System;

namespace AoC._2019.Classes
{
    public class OperationInput
    {
        public Func<long> GetInput { get; }
        public long[] Parameters { get; }

        public OperationInput(Func<long> getInput, params long[] parameters)
        {
            GetInput = getInput;
            Parameters = parameters;
        }
    }
}
