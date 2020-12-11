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
    [Day(9)]
    public class Day09: Puzzle
    {
        private long[] ParseInput(string input) => input.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(i => long.Parse(i)).ToArray();

        private long GetFirstInvalidEntry(long[] entries)
        {
            var indexPairs = new List<int[]>();

            for (var i = -25; i < -1; ++i) for (var j = (i + 1); j < 0; ++j) indexPairs.Add(new int[] { i, j });

            for (var counter = 25; counter < entries.Length; ++counter)
            {
                var valid = false;

                foreach (var indexPair in indexPairs)
                {
                    valid |= (entries[counter] == entries[counter + indexPair[0]] + entries[counter + indexPair[1]]);
                }

                if (!valid) return entries[counter];
            }

            throw new Exception("No invalid entry found");
        }
        protected override string Part1(string input) => GetFirstInvalidEntry(ParseInput(input)).ToString();
        protected override string Part2(string input)
        {
            var parsedInput = ParseInput(input);
            var target = GetFirstInvalidEntry(parsedInput);

            for(var skip = 0; skip < (parsedInput.Length - 1); ++skip)
            {
                for (var take = 2; take <= (parsedInput.Length - skip); ++take)
                {
                    var sequence = parsedInput.Skip(skip).Take(take);
                    if (sequence.Sum() == target) return (sequence.Min() + sequence.Max()).ToString();
                }
            }

            throw new Exception("No valid sequence found");
        }
    }
}

