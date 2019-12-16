using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AoC.Common;
using AoC.Common.Attributes;
using AoC.Common.ExtensionMethods;

using AoC._2019.Classes;
using AoC._2019.Enums;

namespace AoC._2019
{
    [Year(2019)]
    [Day(16)]
    [Test("12345678", "23845678", null)]
    [Test("80871224585914546619083218645595", "24176176", null)]
    [Test("19617804207202209144916044189917", "73745418", null)]
    [Test("69317163492948606335995924319873", "52432133", null)]
    [Test("03036732577212944063491565474664", null, "84462026")]
    //[Test("02935109699940807407585447034323", null, "78725270")]
    //[Test("03081770884921959731165446850517", null, "53553731")]
    public class Day16 : _2019Puzzle
    {
        private int[] ParseInput(string input) => input.Select(c => int.Parse(c.ToString())).ToArray();

        private Dictionary<int, int[]> GetPatternCache(int[] basePattern, int maximumPosition)
        {
            var patternCache = new Dictionary<int, int[]>();

            for (var position = 0; position < maximumPosition; ++position) patternCache.Add(position, basePattern.SelectMany(i => new int[(position + 1)].Populate(i)).ToArray());

            return patternCache;
        }

        private async Task<int> TransformSignalDigit(int[] signal, int digit)
        {
            return await Task.Run(() =>
            {
                var updatedSignalDigit = 0;

                for (var i = digit; i < signal.Length; i += (digit + 1) * 4) for (var j = 0; j <= digit && (i + j) < signal.Length; ++j) updatedSignalDigit += signal[i + j];
                for (var i = (3 * digit) + 2; i < signal.Length; i += (digit + 1) * 4) for (var j = 0; j <= digit && (i + j) < signal.Length; ++j) updatedSignalDigit -= signal[i + j];

                updatedSignalDigit = Math.Abs(updatedSignalDigit);
                updatedSignalDigit = updatedSignalDigit - ((int)Math.Floor(updatedSignalDigit / 10M) * 10);

                return updatedSignalDigit;
            });
        }

        private int[] TransformSignal(int[] signal, int cycles)
        {
            for(var c = 0; c < cycles; ++c)
            {
                var digitCount = 0;
                var digitCalculations = signal.ToDictionary(i => digitCount, i => TransformSignalDigit(signal, digitCount++));

                while (!digitCalculations.All(kvp => kvp.Value.IsCompleted)) Thread.Sleep(1);

                digitCalculations.ForEach(kvp => signal[kvp.Key] = kvp.Value.Result);
            }

            return signal;
        }

        protected override string Part1(string input) => String.Join("", TransformSignal(ParseInput(input), 100).Take(8).Select(i => i.ToString()));
        protected override string Part2(string input)
        {
            var signal = ParseInput(input);
            var skipCount = 0;

            for (var i = 0; i < 7; ++i) skipCount += signal[i] * (int)Math.Pow(10, (6 - i));
            skipCount = skipCount % (input.Length * 10000);

            return String.Join("", TransformSignal((new int[10000][]).Populate(signal).SelectMany(i => i).ToArray(), 100).Skip(skipCount).Take(8).Select(i => i.ToString()));
        }
    }
}
