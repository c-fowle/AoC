using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Common.Attributes;
using AoC.Common.ExtensionMethods;

namespace AoC.Common
{
    public abstract class Puzzle : IPuzzle
    {
        protected int Year { get; }
        protected int Day { get; }
        protected string Input { get; }

        public Puzzle()
        {
            var yearAttributes = this.GetType().GetCustomAttributes(false).OfType<YearAttribute>();
            var dayAttributes = this.GetType().GetCustomAttributes(false).OfType<DayAttribute>();

            if (yearAttributes.Count() != 1) throw new Exception();
            if (dayAttributes.Count() != 1) throw new Exception();

            Year = yearAttributes.Single().YearValue;
            Day = dayAttributes.Single().DayValue;

            Input = Connectivity.FetchInput(Year, Day);
        }

        private PuzzleResult<T> TimePuzzle<T>(Func<T> function, bool autoSubmit)
        {
            var start = DateTime.Now;
            var result = function();
            var executionTime = DateTime.Now - start;

            if (autoSubmit)
            {
                var submissionResponse = Connectivity.SubmitSolution(Year, Day, result.ToString());
            }

            return new PuzzleResult<T>(result, DateTime.Now - start, null);
        }

        protected abstract string Part1(string input);
        public IList<PuzzleResult<bool>> TestPart1() => this.GetType().GetCustomAttributes(false).OfType<TestAttribute>()?.ForEach(puzzleTest => TimePuzzle(() => Part1(puzzleTest.Input) == puzzleTest.Part1Result, false)).ToList();
        public PuzzleResult<string> SolvePart1(bool autoSubmit) => TimePuzzle(() => Part1(Input), autoSubmit);

        protected abstract string Part2(string input);
        public IList<PuzzleResult<bool>> TestPart2() => this.GetType().GetCustomAttributes(false).OfType<TestAttribute>()?.ForEach(puzzleTest => TimePuzzle(() => Part2(puzzleTest.Input) == puzzleTest.Part2Result, false)).ToList();
        public PuzzleResult<string> SolvePart2(bool autoSubmit) => TimePuzzle(() => Part2(Input), autoSubmit);

    }
}
