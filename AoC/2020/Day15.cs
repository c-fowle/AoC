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
    [Day(15)]
    [Test("0,3,6", "436", "175594")]
    public class Day15: Puzzle
    {
        private int[] ParseInput(string input) => input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i)).ToArray();

        protected override string Part1(string input)
        {
            var parsedInput = ParseInput(input);

            var previousNumbers = new Dictionary<int, int>();
            var turn = 1;
            var lastNumber = parsedInput[0];

            while (turn < 2020)
            {
                int thisNumber;
                
                if (turn < parsedInput.Length) thisNumber = parsedInput[turn];
                else if (previousNumbers.ContainsKey(lastNumber)) thisNumber = (turn) - previousNumbers[lastNumber];
                else thisNumber = 0;

                if (!previousNumbers.ContainsKey(lastNumber)) previousNumbers.Add(lastNumber, turn);
                else previousNumbers[lastNumber] = turn;

                lastNumber = thisNumber;
                turn++;
            }

            return lastNumber.ToString();
        }
        protected override string Part2(string input)
        {
            var parsedInput = ParseInput(input);

            var previousNumbers = new Dictionary<int, int>();
            var turn = 1;
            var lastNumber = parsedInput[0];

            while (turn < 30000000)
            {
                int thisNumber;

                if (turn < parsedInput.Length) thisNumber = parsedInput[turn];
                else if (previousNumbers.ContainsKey(lastNumber)) thisNumber = (turn) - previousNumbers[lastNumber];
                else thisNumber = 0;

                if (!previousNumbers.ContainsKey(lastNumber)) previousNumbers.Add(lastNumber, turn);
                else previousNumbers[lastNumber] = turn;

                lastNumber = thisNumber;
                turn++;
            }

            return lastNumber.ToString();
        }
    }
}

