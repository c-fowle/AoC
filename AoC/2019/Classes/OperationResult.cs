namespace AoC._2019.Classes
{
    public class OperationResult
    {
        public bool Exit { get; }
        public int? Output { get; }
        public int? JumpTo { get; }

        public OperationResult(bool exit = false, int? output = null, int? jumpTo = null)
        {
            Exit = exit;
            Output = output;
            JumpTo = jumpTo;
        }
    }
}
