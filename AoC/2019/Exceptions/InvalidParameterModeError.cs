namespace AoC._2019.Exceptions
{
    public class InvalidParameterModeError : IntcodeOperationException
    {
        public InvalidParameterModeError() : base("Unrecognised parameter mode...")
        { }
    }
}
