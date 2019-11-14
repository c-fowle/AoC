using System.Collections.Generic;

namespace AoC.Common
{
    interface IPuzzle
    { 
        IList<PuzzleResult<bool>> TestPart1();
        PuzzleResult<string> SolvePart1(bool autoSubmit);
        IList<PuzzleResult<bool>> TestPart2();
        PuzzleResult<string> SolvePart2(bool autoSubmit);
    }
}
