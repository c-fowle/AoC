using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Common;
using AoC.Common.Attributes;
using AoC.Common.ExtensionMethods;

namespace AoC._2020
{
    [Year(2020)]
    [Day(8)]
    public class Day08: Puzzle
    {
        private List<Tuple<string, int>> ParseInput(string input) => input.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Split(' ')).Select(s => new Tuple<string, int>(s[0], s.Length > 1 ? int.Parse(s[1]) : 0)).ToList();

        private int? RunProgram(List<Tuple<string, int>> commands, bool returnOnLoop = true, int? changeAtLine = null)
        {
            var lineNumber = 0;
            var accumulator = 0;
            var previousCommands = new List<int>();

            while (lineNumber < commands.Count)
            {
                if (previousCommands.Contains(lineNumber)) return returnOnLoop ? (int?)accumulator : null;

                previousCommands.Add(lineNumber);

                var command = (changeAtLine ?? - 1) == lineNumber ? commands[lineNumber].Item1 == "jmp" ? "nop" : "jmp" : commands[lineNumber].Item1;

                switch (command)
                {
                    case "acc":
                        accumulator += commands[lineNumber].Item2;
                        ++lineNumber;
                        break;
                    case "nop":
                        ++lineNumber;
                        break;
                    case "jmp":
                        lineNumber += commands[lineNumber].Item2;
                        break;
                    default:
                        throw new Exception(String.Format("Unexpected command {0} @ line {1}", commands[0], lineNumber));
                }
            }

            return (int?)accumulator;
        }

        protected override string Part1(string input) => RunProgram(ParseInput(input)).ToString();
        protected override string Part2(string input)
        {
            var commands = ParseInput(input);

            var toChange = new List<int>();
            for (var counter = 0; counter < commands.Count; ++counter)
            {
                if ((commands[counter].Item1 == "jmp" || commands[counter].Item1 == "nop") && commands[counter].Item2 !=0)
                {
                    toChange.Add(counter);
                }
            }

            for(var counter = 0; counter < toChange.Count(); ++counter)
            {
                var programResult = RunProgram(commands, false, toChange[counter]);
                if (programResult.HasValue) return programResult.Value.ToString();
            }

            throw new Exception("No valid solution");
        }
    }
}

