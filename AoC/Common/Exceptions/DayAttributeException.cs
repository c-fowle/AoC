using System;

namespace AoC.Common.Exceptions
{
    public class DayAttributeException : Exception
    {
        public DayAttributeException(Type type)
            : base(String.Format("Could not determine day for implementation of Puzzle: {0}", type.Name))
        {
        }
    }
}
