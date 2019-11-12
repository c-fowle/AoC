using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class TestAttribute : Attribute
    {
        public string Input { get; }
        public string Part1Result { get; }
        public string Part2Result { get; }

        public TestAttribute(string input, string part1Result, string part2Result)
        {
            Input = input;
            Part1Result = part1Result;
            Part2Result = part2Result;
        }
    }
}
