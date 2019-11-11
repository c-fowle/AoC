using System;

namespace AoC.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class YearAttribute : Attribute
    {
        public int YearValue { get; }
        public YearAttribute(int yearValue)
        {
            YearValue = yearValue;
        }
    }
}
