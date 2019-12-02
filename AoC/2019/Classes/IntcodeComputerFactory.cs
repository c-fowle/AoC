using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Common.ExtensionMethods;

namespace AoC._2019.Classes
{
    public class IntcodeComputerFactory
    {
        private Dictionary<int, Opcode> OpcodeDefinitions { get; }
        public IntcodeComputerFactory()
        {
            OpcodeDefinitions = new Dictionary<int, Opcode>();
            CreateOpcodes();
        }
        private void CreateOpcodes()
        {
            OpcodeDefinitions.Add(1, new Opcode(1, 3, new Func<int, int[], bool>((pointer, memory) =>
            {
                memory[memory[pointer + 3]] = memory[memory[pointer + 1]] + memory[memory[pointer + 2]];
                return false;
            })));
            OpcodeDefinitions.Add(2, new Opcode(2, 3, new Func<int, int[], bool>((pointer, memory) =>
            {
                memory[memory[pointer + 3]] = memory[memory[pointer + 1]] * memory[memory[pointer + 2]];
                return false;
            })));
            OpcodeDefinitions.Add(99, new Opcode(99, 0, new Func<int, int[], bool>((pointer, memory) => true)));
        }
        public IntcodeComputer CreateIntcodeComputer(int[] initialMemoryState, int[] allowedOperations=null)
        {
            if (allowedOperations == null) allowedOperations = OpcodeDefinitions.Keys.ToArray();

            var operationList = new List<Opcode>();
            allowedOperations.ForEach(i => operationList.Add(OpcodeDefinitions[i]));

            return new IntcodeComputer(operationList, initialMemoryState);
        }
    }
}
