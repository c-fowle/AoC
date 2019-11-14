using AoC.Common.Attributes;
using AoC.Common.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AoC.Common
{
    public abstract class Puzzle : IPuzzle
    {
        private static Tuple<T, TimeSpan> Monitor<T>(Func<T> function)
        {
            var start = DateTime.Now;
            var result = function();
            return new Tuple<T, TimeSpan>(result, DateTime.Now - start);
        }

        protected int Year { get; }
        protected int Day { get; }
        protected string Input { get; }

        public Puzzle()
        {
            var yearAttribute = this.GetType().GetCustomAttribute(typeof(YearAttribute)) as YearAttribute;
            var dayAttribute = this.GetType().GetCustomAttribute(typeof(DayAttribute)) as DayAttribute;

            if (yearAttribute is null) throw new Exception();
            if (dayAttribute is null) throw new Exception();

            Year = yearAttribute.YearValue;
            Day = dayAttribute.DayValue;

            Input = Connectivity.FetchInput(Year, Day);
        }

        private Func<string, string> GetPartFunction(int part)
        {
            if (part == 1) return Part1;
            if (part == 2) return Part2;

            Console.WriteLine("Part number provided was not 1 or 2...");
            return null;
        }

        public IList<PuzzleResult> Test(int part)
        {
            Func<string, string> partFunction = GetPartFunction(part);
            if (partFunction == null) return null;

            return GetType().GetCustomAttributes<TestAttribute>().ForEach(testAttribute =>
            {
                var result = Monitor(() => partFunction(testAttribute.Input));
                var testPassed = result.Item1 == testAttribute.GetResultForPart(part);

                Console.WriteLine("Test {0} (Expected: {1}; Got: {2})", testPassed ? "passed" : "failed", result.Item1, testAttribute.GetResultForPart(part));

                return new PuzzleResult(result.Item1, result.Item2, testPassed);
            }).ToList();
        }

        public PuzzleResult Solve(int part, bool autoSubmit)
        {
            Func<string, string> partFunction = GetPartFunction(part);
            if (partFunction == null) return null;

            var result = Monitor(() => partFunction(Input));
            return new PuzzleResult(result.Item1, result.Item2, autoSubmit ? Connectivity.SubmitSolution(Year, Day, part, result.Item1) : null);
        }

        protected abstract string Part1(string input);
        protected abstract string Part2(string input);
    }
}
