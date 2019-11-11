using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Common.Attributes;
using AoC.Common.ExtensionMethods;

namespace AoC.Common
{
    interface IPuzzle
    { 
        IList<PuzzleResult<bool>> TestPart1();
        PuzzleResult<string> SolvePart1(bool autoSubmit);
        IList<PuzzleResult<bool>> TestPart2();
        PuzzleResult<string> SolvePart2(bool autoSubmit);
    }

    public class PuzzleResult<T>
    {
        public T Solution { get; }
        public TimeSpan ExecutionTime { get; }
        public string SubmissionResponse { get; }

        public PuzzleResult(T solution, TimeSpan executionTime, string submissionResponse)
        {
            Solution = solution;
            ExecutionTime = executionTime;
            SubmissionResponse = submissionResponse;
        }
    }

    public class PuzzleTest
    {
        public string Input { get; }
        public string Part1Result { get; }
        public string Part2Result { get; }

        public PuzzleTest(string input, string part1Result, string part2Result)
        {
            Input = input;
            Part1Result = part1Result;
            Part2Result = part2Result;
        }
    }

    public abstract class Puzzle : IPuzzle
    {
        protected int Year { get; }
        protected int Day { get; }
        protected string Input { get; }
        private IList<PuzzleTest> PuzzleTests { get; }

        public Puzzle(IList<PuzzleTest> puzzleTests)
        {
            var yearAttributes = this.GetType().GetCustomAttributes(false).OfType<YearAttribute>();
            var dayAttributes = this.GetType().GetCustomAttributes(false).OfType<DayAttribute>();

            if (yearAttributes.Count() > 1) throw new Exception();
            if (yearAttributes.Count() == 0) throw new Exception();
            if (dayAttributes.Count() > 1) throw new Exception();
            if (dayAttributes.Count() == 0) throw new Exception();

            Year = yearAttributes.Single().YearValue;
            Day = dayAttributes.Single().DayValue;
            PuzzleTests = new List<PuzzleTest>();

            puzzleTests.ForEach(pt => PuzzleTests.Add(pt));

            Input = Connectivity.FetchInput(Year, Day);
        }

        private PuzzleResult<T> TimePuzzle<T>(Func<T> function, bool autoSubmit)
        {
            var start = DateTime.Now;
            var result = function();
            return new PuzzleResult<T>(result, DateTime.Now - start, null);
        }

        protected abstract string Part1(string input);
        public IList<PuzzleResult<bool>> TestPart1() => PuzzleTests.ForEach(puzzleTest => TimePuzzle(() => Part1(puzzleTest.Input) == puzzleTest.Part1Result, false)).ToList();
        public PuzzleResult<string> SolvePart1(bool autoSubmit) => TimePuzzle(() => Part1(Input), autoSubmit);

        protected abstract string Part2(string input);
        public IList<PuzzleResult<bool>> TestPart2() => PuzzleTests.ForEach(puzzleTest => TimePuzzle(() => Part2(puzzleTest.Input) == puzzleTest.Part2Result, false)).ToList();
        public PuzzleResult<string> SolvePart2(bool autoSubmit) => TimePuzzle(() => Part2(Input), autoSubmit);

    }
}
