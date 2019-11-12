using System;

namespace AoC.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DayAttribute : Attribute
    {
        public int DayValue { get; }
        public DayAttribute(int dayValue)
        {
            DayValue = dayValue;
        }
    }
}
