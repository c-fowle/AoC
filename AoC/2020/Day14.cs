using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Common;
using AoC.Common.Attributes;
using AoC.Common.ExtensionMethods;

using AoC.Common.Helpers;

namespace AoC._2020
{
    interface ICommand
    {

    }

    struct MaskCommand : ICommand
    {
        public string Mask { get; }

        public MaskCommand(string mask)
        {
            Mask = mask;
        }
    }

    struct MemCommand : ICommand
    {
        public string Address { get; }
        public string Value { get; }

        public MemCommand(int address, long value)
        {
            Address = Convert.ToString(address, 2);
            Address = new string('0', (36 - Address.Length)) + Address;
            Value = Convert.ToString(value, 2);
            Value = new string('0', (36 - Value.Length)) + Value;
        }
    }


    [Year(2020)]
    [Day(14)]
    [Test("mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X\nmem[8] = 11\nmem[7] = 101\nmem[8] = 0", "165", null)]
    [Test("mask = 000000000000000000000000000000X1001X\nmem[42] = 100\nmask = 00000000000000000000000000000000X0XX\nmem[26] = 1", null, "208")]
    public class Day14: Puzzle
    {
        private List<ICommand> ParseInput(string input)
        {
            var inputLines = input.Replace("]", "").Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Split( new[] { " = " }, StringSplitOptions.RemoveEmptyEntries)).ToList();

            var commands = new List<ICommand>();

            for(var i = 0; i < inputLines.Count(); ++i)
            {
                if (inputLines[i][0][1] == 'a') commands.Add(new MaskCommand(inputLines[i][1]));
                else commands.Add(new MemCommand(int.Parse(inputLines[i][0].Split('[')[1]), int.Parse(inputLines[i][1])));
            }

            return commands;
        }

        protected override string Part1(string input)
        {
            var parsedInput = ParseInput(input);
            var currentMask = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            var memoryAddresses = new Dictionary<long, long>();
            
            for(var i = 0; i < parsedInput.Count; ++i)
            {
                if (typeof(MaskCommand).IsInstanceOfType(parsedInput[i]))
                {
                    var typed = (MaskCommand)parsedInput[i];
                    currentMask = typed.Mask;
                }
                else
                {
                    var typed = (MemCommand)parsedInput[i];
                    var address = Convert.ToInt64(typed.Address, 2);

                    if (!memoryAddresses.ContainsKey(address)) memoryAddresses.Add(address, 0);

                    var binaryBits = "";

                    for(var j = 0; j < currentMask.Length; ++j)
                    {
                        if (currentMask[j] == 'X') binaryBits += typed.Value[j];
                        else binaryBits += currentMask[j];
                    }

                    memoryAddresses[address] = Convert.ToInt64(binaryBits, 2);

                }
            }

            return memoryAddresses.Values.Sum().ToString();
        }
        protected override string Part2(string input)
        {
            var parsedInput = ParseInput(input);
            var currentMask = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            var memoryAddresses = new Dictionary<string, long>();

            for (var i = 0; i < parsedInput.Count; ++i)
            {
                if (typeof(MaskCommand).IsInstanceOfType(parsedInput[i]))
                {
                    var typed = (MaskCommand)parsedInput[i];
                    currentMask = typed.Mask;
                }
                else
                {
                    var typed = (MemCommand)parsedInput[i];
                    var addresses = new List<string> { "" };

                    for (var j = 0; j < currentMask.Length; ++j)
                    {
                        var newAddresses = new List<string>();

                        if (currentMask[j] == '0') addresses.ForEach(s => newAddresses.Add(s + typed.Address[j]));
                        else if (currentMask[j] == '1') addresses.ForEach(s => newAddresses.Add(s + '1'));
                        else
                        {
                            addresses.ForEach(s =>
                            {
                                newAddresses.Add(s + '0');
                                newAddresses.Add(s + '1');
                            });
                        }

                        addresses = newAddresses;
                    }

                    addresses.ForEach(s =>
                    {
                        if (!memoryAddresses.ContainsKey(s)) memoryAddresses.Add(s, Convert.ToInt64(typed.Value, 2));
                        else memoryAddresses[s] = Convert.ToInt64(typed.Value, 2);
                    });
                }
            }

            return memoryAddresses.Values.Sum().ToString();
        }
    }
}


