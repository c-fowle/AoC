using System;

namespace AoC.Common
{
    public class PuzzleResult
    {
        public string Solution { get; }
        public TimeSpan ExecutionTime { get; }
        public string FullTextResponse { get; }
        public SolutionResponse SolutionResponse { get; }

        public PuzzleResult(string solution, TimeSpan executionTime, string fullTextResponse)
        {
            Solution = solution;
            ExecutionTime = executionTime;
            FullTextResponse = fullTextResponse;
        }
        public PuzzleResult(string solution, TimeSpan executionTime, bool success)
        {
            Solution = solution;
            ExecutionTime = executionTime;
            FullTextResponse = null;
            SolutionResponse = success ? SolutionResponse.Correct : SolutionResponse.IncorrectNoInformation;
        }
    }
}
