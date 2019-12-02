using System;

namespace AoC._2019.Classes
{
    public class Opcode
    {
        public int Code { get; }
        public int ParameterCount { get; }
        public Func<int, int[], bool> Operation { get; }

        public Opcode(int code, int parameterCount, Func<int, int[], bool> operation)
        {
            Code = code;
            ParameterCount = parameterCount;
            Operation = operation;
        }
    }
}
