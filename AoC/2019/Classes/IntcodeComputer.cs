using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AoC._2019.Exceptions;
using AoC.Common.ExtensionMethods;

namespace AoC._2019.Classes
{
    public class IntcodeComputer
    {
        protected IList<Opcode> AvailableOperations { get; }
        private int[] InitialMemory { get; }

        private int InstructionPointer { get; set; }
        public int[] CurrentMemory { get; private set; }
        private Queue<int> Inputs { get; }
        private object InputLock { get; }
        private Queue<int> Outputs { get; }
        private object OutputLock { get; }
        public bool Exited { get; private set; }
        public bool Errored { get; private set; }
        public bool Paused { get; private set; }

        public IntcodeComputer(IList<Opcode> availableOperations, int[] initialMemoryState)
        {
            AvailableOperations = new List<Opcode>();
            availableOperations.ForEach(AvailableOperations.Add);

            InitialMemory = new int[initialMemoryState.Length];
            initialMemoryState.CopyTo(InitialMemory, 0);

            Inputs = new Queue<int>();
            InputLock = new object();

            Outputs = new Queue<int>();
            OutputLock = new object();
        }
        private int[] GetDigits(int number)
        {
            var digitCount = 1;

            while (number > Math.Pow(10, digitCount)) ++digitCount;

            var digits = new int[digitCount];

            for (var d = (digitCount - 1); d > -1; --d)
            {
                var thisDigit = (int)Math.Floor((double)number / (Math.Pow(10, d)));
                digits[digitCount - (d + 1)] = thisDigit;
                number = (int)(number - (thisDigit * (Math.Pow(10, d))));
            }

            return digits;
        }

        public void ForceStop()
        {
            Exited = true;
            Errored = true;
        }

        private int GetNextInput()
        {
            while (!Exited)
            {
                lock (InputLock)
                {
                    if (Inputs.Count > 0)
                    {
                        Paused = false;
                        return Inputs.Dequeue();
                    }
                }
                Paused = true;
                Thread.Sleep(100);
            }

            throw new InterruptedWhileAwaitingInputError();
        }

        public void AddInput(int input)
        {
            lock (InputLock)
            {
                Inputs.Enqueue(input);
            }
        }

        public bool OutputReady()
        {
            var ready = false;
            lock (OutputLock)
            {
                ready = Outputs.Count > 0;
            }
            return ready;
        }
        public int? GetLastOutput()
        {
            lock (OutputLock)
            {
                if (Outputs.Count > 0) return Outputs.Dequeue();
            }
            return null;
        }

        private void AddOutput(int output)
        {
            lock(OutputLock)
            {
                Outputs.Enqueue(output);
            }
        }

        public async Task<IntcodeProgramResult> RunProgram(IntcodeProgramInput programInput)
        {
            CurrentMemory = new int[InitialMemory.Length];
            InitialMemory.CopyTo(CurrentMemory, 0);
            programInput.MemoryInitialisation(CurrentMemory);
            
            InstructionPointer = 0;

            Exited = false;
            Errored = false;
            Paused = false;

            Inputs.Clear();
            programInput.Inputs.ForEach(AddInput);

            while (!Exited)
            {
                var operationCode = (int)(((CurrentMemory[InstructionPointer] / (decimal)100) - Math.Floor(CurrentMemory[InstructionPointer] / (decimal)100)) * (decimal)100);
                var matchedOperations = AvailableOperations.Where(op => op.Code == operationCode);

                if (matchedOperations.Count() == 0)
                {
                    Exited = true;
                    Errored = true;
                }
                else if (matchedOperations.Count() == 1)
                {
                    try
                    {
                        var operationResult = await matchedOperations.Single().RunOperation(InstructionPointer, CurrentMemory, GetNextInput);
                        Exited |= operationResult.Exit;
                        if (operationResult.Output.HasValue) AddOutput(operationResult.Output.Value);
                        if (operationResult.JumpTo.HasValue) InstructionPointer = operationResult.JumpTo.Value;
                        else InstructionPointer += (1 + matchedOperations.Single().ParameterCount);
                    }
                    catch (IntcodeOperationException ex)
                    {
                        Exited = true;
                        Errored = true;
                    }
                }
                else throw new Exception("Multiple operation definitions for operation code: " + CurrentMemory[InstructionPointer]);
            }

            var finalMemState = new int[InitialMemory.Length];
            CurrentMemory.CopyTo(finalMemState, 0);

            return new IntcodeProgramResult(!Errored, finalMemState, Outputs.CloneAsList().ToList());
        }
    }
}
