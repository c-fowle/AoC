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
        private Func<OperationInput, long[], OperationResult> Operation { get; }

        public Opcode(int code, int parameterCount, int? writeAddressParameter, Func<OperationInput, long[], OperationResult> operation)
        {
            Code = code;
            ParameterCount = parameterCount;
            WriteAddressParameter = writeAddressParameter;
            Operation = operation;
        }

        public async Task<OperationResult> RunOperation(ulong pointer, long[] memory, long relativeBase, Func<long> getInput)
        {
            var parameters = new long[ParameterCount];

            for(var i = 0; i < ParameterCount; ++i)
            {
                var trimedInstruction = (decimal)Math.Floor(memory[pointer] / Math.Pow(10, i + 2));
                var parameterMode = (int)(((trimedInstruction / (decimal)10) - Math.Floor(trimedInstruction / (decimal)10)) * (decimal)10);

                switch(parameterMode)
                {
                    case 0:
                        if (WriteAddressParameter.HasValue && i == WriteAddressParameter) parameters[i] = memory[pointer + (ulong)(i + 1)];
                        else parameters[i] = memory[memory[pointer + (ulong)(i + 1)]];
                        break;
                    case 1:
                        parameters[i] = memory[pointer + (ulong)(i + 1)];
                        break;
                    case 2:
                        if (WriteAddressParameter.HasValue && i == WriteAddressParameter) parameters[i] = memory[pointer + (ulong)(i + 1)] + relativeBase;
                        else parameters[i] = memory[memory[pointer + (ulong)(i + 1)] + relativeBase];
                        break;
                    default:
                        throw new InvalidParameterModeError();
                }
            }

            return await Task.Run(() => Operation(new OperationInput(getInput, parameters), memory));
        }
    }
}
