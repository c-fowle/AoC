using System;

namespace AoC._2019.Classes
{
    public class OperationInput
    {
        public Func<int> GetInput { get; }
        public int[] Parameters { get; }

        public OperationInput(Func<int> getInput, params int[] parameters)
        {
            GetInput = getInput;
            Parameters = parameters;
        }
    }
}
