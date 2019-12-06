using System;
using System.Collections.Generic;
using System.Linq;

using AoC._2019.Exceptions;
using AoC.Common.ExtensionMethods;

namespace AoC._2019.Classes
{
    public class IntcodeComputer
    {
        protected IList<Opcode> AvailableOperations { get; }
        private int[] InitialMemory { get; }

        public IntcodeComputer(IList<Opcode> availableOperations, int[] initialMemoryState)
        {
            AvailableOperations = new List<Opcode>();
            availableOperations.ForEach(AvailableOperations.Add);

            InitialMemory = new int[initialMemoryState.Length];
            initialMemoryState.CopyTo(InitialMemory, 0);
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

        public IntcodeProgramResult RunProgram(IntcodeProgramInput programInput)
        {
            var outputs = new List<int>();
            var currentMemory = new int[InitialMemory.Length];
            InitialMemory.CopyTo(currentMemory, 0);
            programInput.MemoryInitialisation(currentMemory);

            var instructionPointer = 0;

            var exited = false;
            var errorEncountered = false;
            var inputPosition = 0;

            while (!exited)
            {
                var operationCode = (int)(((currentMemory[instructionPointer] / (decimal)100) - Math.Floor(currentMemory[instructionPointer] / (decimal)100)) * (decimal)100);
                var matchedOperations = AvailableOperations.Where(op => op.Code == operationCode);

                if (matchedOperations.Count() == 0)
                {
                    exited = true;
                    errorEncountered = true;
                }
                else if (matchedOperations.Count() == 1)
                {
                    try
                    {
                        var operationResult = matchedOperations.Single().RunOperation(instructionPointer, currentMemory, programInput.Inputs[inputPosition]);
                        inputPosition = (++inputPosition) % programInput.Inputs.Length;
                        exited = operationResult.Exit;
                        if (operationResult.Output.HasValue) outputs.Add(operationResult.Output.Value);
                        if (operationResult.JumpTo.HasValue) instructionPointer = operationResult.JumpTo.Value;
                        else instructionPointer += (1 + matchedOperations.Single().ParameterCount);
                    }
                    catch (InvalidParameterModeError ex)
                    {
                        exited = true;
                        errorEncountered = true;
                    }
                }
                else throw new Exception("Multiple operation definitions for operation code: " + currentMemory[instructionPointer]);
            }

            return new IntcodeProgramResult(!errorEncountered, currentMemory, outputs);
        }
    }
}
