using System;

namespace AoC._2019.Exceptions
{
    public class InvalidParameterModeError : Exception
    {
        public InvalidParameterModeError() : base("Unrecognised parameter mode...")
        { }
    }
}
