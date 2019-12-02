using System.Collections.Generic;

namespace AoC.Common
{
    interface IPuzzle
    {
        IList<SubmittedSolution> SolutionHistory { get; }
        IList<PuzzleResult> Test(int part);
        PuzzleResult Solve(int part, bool autoSubmit, IList<SubmittedSolution> solutionHistory);
    }
}
