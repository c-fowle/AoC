using System;
using System.Threading.Tasks;

using AoC._2019.Exceptions;

namespace AoC._2019.Classes
{
    public class Opcode
    {
        public int Code { get; }
        public int ParameterCount { get; }
        private int? WriteAddressParameter { get; }
        private Func<OperationInput, int[], OperationResult> Operation { get; }

        public Opcode(int code, int parameterCount, int? writeAddressParameter, Func<OperationInput, int[], OperationResult> operation)
        {
            Code = code;
            ParameterCount = parameterCount;
            WriteAddressParameter = writeAddressParameter;
            Operation = operation;
        }

        public async Task<OperationResult> RunOperation(int pointer, int[] memory, Func<int> getInput)
        {
            var parameters = new int[ParameterCount];

            for(var i = 0; i < ParameterCount; ++i)
            {
                var trimedInstruction = (decimal)Math.Floor(memory[pointer] / Math.Pow(10, i + 2));
                var parameterMode = (int)(((trimedInstruction / (decimal)10) - Math.Floor(trimedInstruction / (decimal)10)) * (decimal)10);

                if (WriteAddressParameter.HasValue && i == WriteAddressParameter) parameterMode = 1;

                switch(parameterMode)
                {
                    case 0:
                        parameters[i] = memory[memory[pointer + (i + 1)]];
                        break;
                    case 1:
                        parameters[i] = memory[pointer + (i + 1)];
                        break;
                    default:
                        throw new InvalidParameterModeError();
                }
            }

            return await Task.Run(() => Operation(new OperationInput(getInput, parameters), memory));
        }
    }
}
