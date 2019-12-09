namespace AoC._2019.Classes
{
    public class OperationResult
    {
        public bool Exit { get; }
        public long? Output { get; }
        public ulong? JumpTo { get; }
        public long? AdjustRelativeBase { get; }

        public OperationResult(bool exit = false, long? output = null, ulong? jumpTo = null, long? adjustRelativeBase = null)
        {
            Exit = exit;
            Output = output;
            JumpTo = jumpTo;
            AdjustRelativeBase = adjustRelativeBase;
        }
    }
}
