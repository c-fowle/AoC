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
    [Day(16)]
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

        private int[] TransformSignal(int[] signal, int cycles)
        {
            //var indexCache = new Dictionary<int, List<int>>();

            //for (var i = 0; i < signal.Length; ++i)
            //{
            //    var theseIndexes = new List<int>();
            //    for (var j = i; j < signal.Length; j += ((i + 1) * 4)) theseIndexes.Add(k);
            //    indexCache.Add(i, theseIndexes);
            //}

            //originalSignal.CopyTo(signal, 0);

            //for (var cycleCounter = 0; cycleCounter < cycles; ++cycleCounter)
            //{
            //    var updatedSignal = new int[signal.Length];

            //    for (var positionCounter = 0; positionCounter < signal.Length; ++positionCounter)
            //    {
            //        var subIndexAdjustment = (2 * positionCounter) + 2;
            //        var positionValue = Math.Abs(indexCache[positionCounter].Sum(i => signal[i]) - indexCache[positionCounter].Sum(i => signal[i + subIndexAdjustment]));
            //        positionValue = positionValue - ((int)Math.Floor(positionValue / 10m) * 10);
            //        updatedSignal[positionCounter] = positionValue;
            //    }

            //    updatedSignal.CopyTo(signal, 0);
            //}

            for (var cycleCounter = 0; cycleCounter < cycles; ++cycleCounter)
            {
                var updatedSignal = new int[signal.Length];

                for (var i = 0; i < signal.Length; ++i)
                {
                    var swapPoint = (int)Math.Ceiling(i / 2m);

                    // Each will need to be determined in this group
                    for (var j = 0; j < swapPoint; ++j)
                    {
                        var comparison = (int)Math.Floor((j + 1m) / (i + 1m));
                        if (comparison % 2 == 0) continue; // even indexes are always ignored
                        if ((comparison - 1) % 4 == 0) updatedSignal[i] += signal[j];
                        else updatedSignal[i] -= signal[j];
                    }

                    // This group will always be added
                    for (var j = swapPoint; j <= i; ++j) updatedSignal[i] += signal[j];

                    updatedSignal[i] = int.Parse(Math.Abs(updatedSignal[i]).ToString().Last().ToString());
                }

                updatedSignal.CopyTo(signal, 0);
                updatedSignal = null;
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
