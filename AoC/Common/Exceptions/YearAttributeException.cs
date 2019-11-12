using System;

namespace AoC.Common.Exceptions
{
    public class YearAttributeException : Exception
    {
        public YearAttributeException(Type type)
            : base(String.Format("Could not determine year for implementation of Puzzle: {0}", type.Name))
        {
        }
    }
}
