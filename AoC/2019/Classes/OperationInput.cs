namespace AoC._2019.Classes
{
    public class OperationInput
    {
        public int Input { get; }
        public int[] Parameters { get; }

        public OperationInput(int input, params int[] parameters)
        {
            Input = input;
            Parameters = parameters;
        }
    }
}
