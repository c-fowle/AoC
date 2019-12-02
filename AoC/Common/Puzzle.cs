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

        public IList<SubmittedSolution> SolutionHistory { get; }
        protected int Year { get; }
        protected int Day { get; }
        protected string Input { get; }

        public Puzzle()
        {
            var yearAttribute = this.GetType().GetCustomAttribute(typeof(YearAttribute)) as YearAttribute;
            var dayAttribute = this.GetType().GetCustomAttribute(typeof(DayAttribute)) as DayAttribute;

            if (yearAttribute is null) throw new Exception();
            if (dayAttribute is null) throw new Exception();

            SolutionHistory = new List<SubmittedSolution>();

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
                if (testAttribute.GetResultForPart(part) == null) return null;

                var result = Monitor(() => partFunction(testAttribute.Input));
                var testPassed = result.Item1 == testAttribute.GetResultForPart(part);

                return new PuzzleResult(result.Item1, result.Item2, "Expected: " + testAttribute.GetResultForPart(part) + "; Got: " + result.Item1, testPassed);
            }).Where(pr => pr != null).ToList();
        }

        public PuzzleResult Solve(int part, bool autoSubmit, IList<SubmittedSolution> solutionHistory)
        {
            Func<string, string> partFunction = GetPartFunction(part);
            if (partFunction == null) return null;

            var result = Monitor(() => partFunction(Input));

            var previousMatchingSolution = solutionHistory.FirstOrDefault(ss => ss.Solution == result.Item1);
            if (previousMatchingSolution != null) return new PuzzleResult(result.Item1, result.Item2, previousMatchingSolution.Response, true);

            var previousCorrectSolution = solutionHistory.FirstOrDefault(ss => ss.Response == SolutionResponse.Correct);
            if (previousCorrectSolution != null)
            {
                var parsedCorrectSolution = 0;
                var parsedCurrentSolution = 0;

                if (int.TryParse(previousCorrectSolution.Solution, out parsedCorrectSolution) && int.TryParse(result.Item1, out parsedCurrentSolution))
                {
                    if (parsedCurrentSolution > parsedCorrectSolution) return new PuzzleResult(result.Item1, result.Item2, SolutionResponse.IncorrectTooHigh, false);
                    if (parsedCurrentSolution < parsedCorrectSolution) return new PuzzleResult(result.Item1, result.Item2, SolutionResponse.IncorrectTooLow, false);
                }

                return new PuzzleResult(result.Item1, result.Item2, SolutionResponse.IncorrectNoInformation, false);
            }

            return new PuzzleResult(result.Item1, result.Item2, autoSubmit ? Connectivity.SubmitSolution(Year, Day, part, result.Item1) : null);
        }

        protected abstract string Part1(string input);
        protected abstract string Part2(string input);
    }
}
