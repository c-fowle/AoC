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
            // OPCODE 1 - ADDITION: x [p1, p2, p3] => set mem[p3] = p1 + p2
            OpcodeDefinitions.Add(1, new Opcode(1, 3, 2, new Func<OperationInput, int[], OperationResult>((input, memory) =>
            {
                memory[input.Parameters[2]] = input.Parameters[0] + input.Parameters[1];
                return new OperationResult();
            })));

            // OPCODE 2 - MULTIPLICATION: x [p1, p2, p3] => set mem[p3] = p1 * p2
            OpcodeDefinitions.Add(2, new Opcode(2, 3, 2, new Func<OperationInput, int[], OperationResult>((input, memory) =>
            {
                memory[input.Parameters[2]] = input.Parameters[0] * input.Parameters[1];
                return new OperationResult();
            })));

            // OPCODE 3 - INPUT TO ADDRESS: x [p1] => set mem[p1] = I
            OpcodeDefinitions.Add(3, new Opcode(3, 1, 0, new Func<OperationInput, int[], OperationResult>((input, memory) =>
            {
                memory[input.Parameters[0]] = input.GetInput();
                return new OperationResult();
            })));

            // OPCODE 4 - OUPUT FROM ADDRESS: x [p1] => out p1
            OpcodeDefinitions.Add(4, new Opcode(4, 1, null, new Func<OperationInput, int[], OperationResult>((input, memory) =>
            {
                return new OperationResult(output: input.Parameters[0]);
            })));

            // OPCODE 5 - JUMP IF TRUE: x [p1, p2] => if p1 != 0; jump to p2 
            OpcodeDefinitions.Add(5, new Opcode(5, 2, null, new Func<OperationInput, int[], OperationResult>((input, memory) =>
            {
                return new OperationResult(jumpTo: input.Parameters[0] != 0 ? (int?)input.Parameters[1] : null);
            })));

            // OPCODE 6 - JUMP IF FALSE: x [p1, p2] => if p1 == 0; jump to p2
            OpcodeDefinitions.Add(6, new Opcode(6, 2, null, new Func<OperationInput, int[], OperationResult>((input, memory) =>
            {
                return new OperationResult(jumpTo: input.Parameters[0] == 0 ? (int?)input.Parameters[1] : null);
            })));

            // OPCODE 7 - LESS THAN: x [p1, p2, p3] => set mem[p3] = 1 if p1 < p2; 0 otherwise
            OpcodeDefinitions.Add(7, new Opcode(7, 3, 2, new Func<OperationInput, int[], OperationResult>((input, memory) =>
            {
                memory[input.Parameters[2]] = input.Parameters[0] < input.Parameters[1] ? 1 : 0;
                return new OperationResult();
            })));

            // OPCODE 8 - EQUAL TO: x [p1, p2, p3] => set mem[p3] = 1 if p1 == p2; 0 otherwise
            OpcodeDefinitions.Add(8, new Opcode(8, 3, 2, new Func<OperationInput, int[], OperationResult>((input, memory) =>
            {
                memory[input.Parameters[2]] = input.Parameters[0] == input.Parameters[1] ? 1 : 0;
                return new OperationResult();
            })));

            // OPCODE 99 - EXIT CODE: x => return exit signal
            OpcodeDefinitions.Add(99, new Opcode(99, 0, null, new Func<OperationInput, int[], OperationResult>((input, memory) => new OperationResult(true))));
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
