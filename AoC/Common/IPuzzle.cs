using System.Collections.Generic;

namespace AoC.Common
{
    interface IPuzzle
    {
        IList<PuzzleResult> Test(int part);
        PuzzleResult Solve(int part, bool autoSubmit, IList<SubmittedSolution> solutionHistory);
    }
}
