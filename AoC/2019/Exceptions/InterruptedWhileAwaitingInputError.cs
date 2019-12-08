namespace AoC._2019.Exceptions
{
    public class InterruptedWhileAwaitingInputError : IntcodeOperationException
    {
        public InterruptedWhileAwaitingInputError() : base("The intcode computer operation was interrupted while awaiting a new input")
        { }
    }
}
