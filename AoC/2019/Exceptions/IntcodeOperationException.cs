using System;

namespace AoC._2019.Exceptions
{
    public class IntcodeOperationException : Exception
    {
        public IntcodeOperationException(string message) : base(message)
        { }
    }
}
