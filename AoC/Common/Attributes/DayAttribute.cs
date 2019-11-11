using System;

namespace AoC.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DayAttribute : Attribute
    {
        public int DayValue { get; }
        public DayAttribute(int dayValue)
        {
            DayValue = dayValue;
        }
    }
}
