namespace AoC._2019.Exceptions
{
    public class InterruptedWhileAwaitingOutputError : IntcodeOperationException
    {
        public InterruptedWhileAwaitingOutputError() : base("The intcode computer operation was interrupted while awaiting a new output")
        { }
    }
}
