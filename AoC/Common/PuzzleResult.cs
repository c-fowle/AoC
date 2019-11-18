using System;
using System.Text.RegularExpressions;

namespace AoC.Common
{
    public class PuzzleResult
    {
        public string Solution { get; }
        public TimeSpan ExecutionTime { get; }
        public string FullTextResponse { get; }
        public SolutionResponse SolutionResponse { get; }
        public bool RepeatedSubmission { get; }
        public bool ComparedWithCached { get; }

        public TimeSpan? WaitTime { get; set; }

        private SolutionResponse GetSolutionResponseFromText(string textResponse)
        {
            var solutionParser = new Regex(@"That's (not )?the right answer", RegexOptions.Singleline);
            var responseMatches = solutionParser.Matches(textResponse);

            if (responseMatches.Count == 1)
            {
                var match = responseMatches[0];

                if (match.Value == "That's the right answer") return SolutionResponse.Correct;
                else
                {
                    var solutionComparisonParser = new Regex(@"; your answer is too (high|low)", RegexOptions.Singleline);
                    var responseComparisonMatches = solutionComparisonParser.Matches(textResponse);

                    if (responseComparisonMatches.Count == 1)
                    {
                        var comparisonMatch = responseComparisonMatches[0];

                        if (comparisonMatch.Value == "; your answer is too high") return SolutionResponse.IncorrectTooHigh;
                        else return SolutionResponse.IncorrectTooLow;
                    }
                    return SolutionResponse.IncorrectNoInformation;
                }
            }

            var waitParser = new Regex("You gave an answer too recently", RegexOptions.Singleline);
            var waitMatches = waitParser.Matches(textResponse);

            if (waitMatches.Count == 1)
            {
                var timerParser = new Regex(@"You have ((?<minutes>\d*)m )?(?<seconds>\d*)s left to wait", RegexOptions.Singleline);
                var timerMatches = timerParser.Matches(textResponse);

                if (timerMatches.Count == 1) WaitTime = new TimeSpan(0, int.Parse((timerMatches[0].Groups["minutes"]?.Value ?? "") == "" ? "0" : timerMatches[0].Groups["minutes"]?.Value), int.Parse(timerMatches[0].Groups["seconds"].Value));

                return SolutionResponse.WaitToSubmit;
            }

            return SolutionResponse.Unrecognised;
        }

        public PuzzleResult(string solution, TimeSpan executionTime, string fullTextResponse)
        {
            Solution = solution;
            ExecutionTime = executionTime;
            FullTextResponse = fullTextResponse;
            SolutionResponse = GetSolutionResponseFromText(FullTextResponse);
            RepeatedSubmission = false;
            ComparedWithCached = false;
        }
        public PuzzleResult(string solution, TimeSpan executionTime, string fullTextDetail, bool success)
        {
            Solution = solution;
            ExecutionTime = executionTime;
            FullTextResponse = fullTextDetail;
            SolutionResponse = success ? SolutionResponse.Correct : SolutionResponse.IncorrectNoInformation;
            RepeatedSubmission = false;
            ComparedWithCached = false;
        }
        public PuzzleResult(string solution, TimeSpan executionTime, SolutionResponse solutionResponse, bool repeated)
        {
            Solution = solution;
            ExecutionTime = executionTime;
            FullTextResponse = null;
            SolutionResponse = solutionResponse;
            RepeatedSubmission = repeated;
            ComparedWithCached = !repeated;
        }
    }
}
