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
        private IList<Opcode> AvailableOperations { get; }
        private long[] InitialMemory { get; }
        private long? DefaultInput { get; }

        private ulong InstructionPointer { get; set; }
        private long RelativeBase { get; set; }
        public long[] CurrentMemory { get; private set; }
        private Queue<long> Inputs { get; }
        private object InputLock { get; }
        private Queue<long> Outputs { get; }
        private object OutputLock { get; }
        public bool Exited { get; private set; }
        public bool Errored { get; private set; }
        public bool Paused { get; private set; }
        public bool OutputReady
        {
            get
            {
                var ready = false;
                lock (OutputLock)
                {
                    ready = Outputs.Count > 0;
                }
                return ready;
            }
        }

        public IntcodeComputer(IList<Opcode> availableOperations, long[] initialMemoryState, long? defaultInput = null)
        {
            AvailableOperations = new List<Opcode>();
            availableOperations.ForEach(AvailableOperations.Add);

            InitialMemory = new long[initialMemoryState.Length];
            initialMemoryState.CopyTo(InitialMemory, 0);

            Inputs = new Queue<long>();
            InputLock = new object();

            Outputs = new Queue<long>();
            OutputLock = new object();

            DefaultInput = defaultInput;
        }

        private long GetNextInput()
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
                    else if (DefaultInput.HasValue) return DefaultInput.Value;
                }
                Paused = true;
                Thread.Sleep(1);
            }

            throw new InterruptedWhileAwaitingInputError();
        }

        public void AddInput(long input)
        {
            lock (InputLock)
            {
                Inputs.Enqueue(input);
            }
        }

        public void AddInputs(long[] inputs)
        {
            lock (InputLock)
            {
                inputs.ForEach(Inputs.Enqueue);
            }
        }

        public long? GetNextOutput()
        {
            var nextOutput = default(long?);

            while (!nextOutput.HasValue)
            {
                if (OutputReady)
                {
                    lock (OutputLock)
                    {
                        if (Outputs.Count > 0) return Outputs.Dequeue();
                    }
                }
                Thread.Sleep(1);
                if (!OutputReady && (Exited || Paused)) break;
            }
            return null;
        }

        private void AddOutput(long output)
        {
            lock(OutputLock)
            {
                Outputs.Enqueue(output);
            }
        }

        public async Task<IntcodeProgramResult> RunProgram(IntcodeProgramInput programInput)
        {
            CurrentMemory = new long[InitialMemory.Length * 100];
            InitialMemory.CopyTo(CurrentMemory, 0);
            programInput.MemoryInitialisation(CurrentMemory);
            
            InstructionPointer = 0;
            RelativeBase = 0;

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
                        var operationResult = await matchedOperations.Single().RunOperation(InstructionPointer, CurrentMemory, RelativeBase, GetNextInput);
                        Exited |= operationResult.Exit;
                        if (operationResult.Output.HasValue) AddOutput(operationResult.Output.Value);
                        if (operationResult.JumpTo.HasValue) InstructionPointer = operationResult.JumpTo.Value;
                        else InstructionPointer += (1 + (ulong)matchedOperations.Single().ParameterCount);
                        if (operationResult.AdjustRelativeBase.HasValue) RelativeBase = RelativeBase + operationResult.AdjustRelativeBase.Value;
                    }
                    catch (IntcodeOperationException ex)
                    {
                        Exited = true;
                        Errored = true;
                    }
                }
                else throw new Exception("Multiple operation definitions for operation code: " + CurrentMemory[InstructionPointer]);
            }

            var finalMemState = new long[InitialMemory.Length * 100];
            CurrentMemory.CopyTo(finalMemState, 0);

            List<long> finalOutputs;

            lock(OutputLock)
            {
                finalOutputs = Outputs.CloneAsList().ToList();
            }

            return new IntcodeProgramResult(!Errored, finalMemState, finalOutputs);
        }
    }
}
