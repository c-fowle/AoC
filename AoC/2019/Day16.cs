using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AoC.Common;
using AoC.Common.Attributes;
using AoC.Common.ExtensionMethods;
using AoC.Common.Helpers;

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
        private async Task<int> TransformDigitAsync(int[] signal, List<int> indexes, int digitCounter)
        {
            return await Task.Run(() =>
            {
                //var patternRepetitionLength = (digitCounter + 1) * 4;
                var subtractionIndexAdjustment = (2 * digitCounter) + 2;
                //var repeatPoint = MathHelper.GetLowestCommonMultiple(signalRepetitionLength, patternRepetitionLength);
                //repeatPoint = repeatPoint > signal.Length ? signal.Length : repeatPoint;

                var transformationTotal = indexes.Sum(i =>
                {
                    var indexTotal = 0;
                    for (var j = 0; j <= digitCounter && i + j < signal.Length; ++j) indexTotal += signal[i + j] - (i + j + subtractionIndexAdjustment < signal.Length ? signal[i + j + subtractionIndexAdjustment] : 0);
                    return indexTotal;
                });

                //for (var i = digitCounter; i < repeatPoint; i += patternRepetitionLength) for (var j = 0; j <= digitCounter && (i + j) < repeatPoint; ++j) transformationTotal += signal[i + j];
                //for (var i = (3 * digitCounter) + 2; i < repeatPoint; i += patternRepetitionLength) for (var j = 0; j <= digitCounter && (i + j) < repeatPoint; ++j) transformationTotal -= signal[i + j];

                transformationTotal = Math.Abs(transformationTotal);

                return (int)(transformationTotal - (long)(Math.Floor(transformationTotal / 10M) * 10));
            });
        }
        private int[] TransformSignal(int[] signal, int startingDigit, int cycles)
        {
            var digitCounter = 0;
            var indexLookup = signal.ToDictionary(x => (digitCounter++ + startingDigit), x =>
            {
                var patternRepetitionLength = (digitCounter + startingDigit) * 4;
                var indexes = new List<int>();
                for (var i = (digitCounter - 1); i < signal.Length; i += patternRepetitionLength) indexes.Add(i);

                return indexes;
            });


            for (var cycleCounter = 0; cycleCounter < cycles; ++cycleCounter)
            {
                digitCounter = 0;
                var transformations = signal.ToDictionary(i => digitCounter, i => TransformDigitAsync(signal, indexLookup[digitCounter + startingDigit], (digitCounter++ + startingDigit)));

                while (transformations.Any(kvp => !kvp.Value.IsCompleted)) Thread.Sleep(1);

                transformations.ForEach(kvp => signal[kvp.Key] = kvp.Value.Result);
                transformations.ForEach(kvp => kvp.Value.Dispose());
                transformations = null;

            }

            return signal;
        }

        protected override string Part1(string input) => String.Join("", TransformSignal(ParseInput(input), 0, 100).Take(8).Select(i => i.ToString()));
        protected override string Part2(string input)
        {
            var signal = ParseInput(input);
            var fullSignal = (new int[10000]).SelectMany(i => signal.CloneAsList());
            var skipCount = 0;

            for (var i = 0; i < 7; ++i) skipCount += signal[i] * (int)Math.Pow(10, (6 - i));
            skipCount = skipCount % (input.Length * 10000);

            return String.Join("", TransformSignal(fullSignal.Skip(skipCount).ToArray(), skipCount, 100).Take(8).Select(i => i.ToString()));
        }
    }
}
