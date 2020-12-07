using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Common;
using AoC.Common.Attributes;
using AoC.Common.ExtensionMethods;

using AoC._2019.Classes;
using AoC._2019.Enums;

namespace AoC._2019
{
    [Year(2019)]
    [Day(22)]
    [Test("", null, "")]
    public class Day22 : _2019Puzzle
    {
        private string[] ParseInput(string input) => input.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        private long dealNewStack(long deckSize, long positionToTrack) => (deckSize - 1) - positionToTrack;
        private long cut(long deckSize, long positionToTrack, long cutAt)
        {
            if (cutAt > 0)
            {
                if (positionToTrack < cutAt) return deckSize - (cutAt - positionToTrack);
                return positionToTrack - cutAt;
            }

            var modCutAt = Math.Abs(cutAt);

            if (positionToTrack >= deckSize - modCutAt) return positionToTrack - (deckSize - modCutAt);
            return positionToTrack + modCutAt;
        }
        private long dealWithIncrement(long deckSize, long positionToTrack, long increment) => (positionToTrack * increment) % deckSize;

        private string[] ReduceInstructions(string[] instructions, long deckSize)
        {
            var reverse = (instructions.Count(i => i == "deal into new stack") % 2) == 1;
            var reducedInstructions = reverse ? new string[] { null, null, "deal into new stack" } : new string[] { null, null };

            var totalIncrement = 1L;
            var dealWithIncrementInstructions = instructions.Where(i => i.StartsWith("deal with increment"));
            var dealWithIncrementIncrements = dealWithIncrementInstructions.Select(i => long.Parse(i.Split(' ').Last()));

            dealWithIncrementIncrements.ForEach(i => totalIncrement = (totalIncrement * i) % deckSize);

            var totalCut = instructions.Where(i => i.StartsWith("cut")).Sum(i => long.Parse(i.Split(' ').Last())) % deckSize;

            reducedInstructions[0] = "deal with increment " + totalIncrement.ToString();
            reducedInstructions[1] = "cut " + totalCut.ToString();

            return reducedInstructions;
        }

        private long TrackPosition(string[] shuffleInstructions, long deckLength, long positionToTrack)
        {
            shuffleInstructions.ForEach(instruction =>
            {
                if (instruction == "deal into new stack") positionToTrack = dealNewStack(deckLength, positionToTrack);
                else if (instruction.StartsWith("cut")) positionToTrack = cut(deckLength, positionToTrack, long.Parse(instruction.Split(' ').Last()));
                else positionToTrack = dealWithIncrement(deckLength, positionToTrack, long.Parse(instruction.Split(' ').Last()));
            });

            return positionToTrack;
        }

        protected override string Part1(string input) => TrackPosition(ReduceInstructions(ParseInput(input), 10007), 10007, 2019).ToString();

        protected override string Part2(string input)
        {
            var initialPosition = 2020L;
            var repetitionCount = 101741582076661;
            var deckSize = 119315717514047;
            var shuffleLookup = new Dictionary<long, long>();
            var shuffleInstructions = ReduceInstructions(ParseInput(input), deckSize);

            var movements = new List<long>();
            var currentPosition = initialPosition;

            throw new Exception();

            return currentPosition.ToString();
        }
    }
}
