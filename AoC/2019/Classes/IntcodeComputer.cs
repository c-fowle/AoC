using System;
using System.Collections.Generic;
using System.Linq;
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

        public int[] RunProgram(Action<int[]> initialisation)
        {
            var currentMemory = new int[InitialMemory.Length];
            InitialMemory.CopyTo(currentMemory, 0);
            initialisation(currentMemory);

            var instructionPointer = 0;

            var exited = false;
            var errorEncountered = false;

            while (!exited)
            {
                var matchedOperations = AvailableOperations.Where(op => op.Code == currentMemory[instructionPointer]);

                if (matchedOperations.Count() == 0)
                {
                    exited = true;
                    errorEncountered = true;
                }
                else if (matchedOperations.Count() == 1)
                {
                    exited = matchedOperations.Single().Operation(instructionPointer, currentMemory);
                    instructionPointer += (1 + matchedOperations.Single().ParameterCount);
                }
                else throw new Exception("Multiple operation definitions for operation code: " + currentMemory[instructionPointer]);
            }

            return errorEncountered ? null : currentMemory;
        }
    }
}
