using System;

namespace AoC.Common
{
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
}
