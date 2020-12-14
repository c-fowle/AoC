using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using AoC.Common;
using AoC.Common.Attributes;
using AoC.Common.ExtensionMethods;
using AoC.Common.Helpers;

namespace AoC._2020
{
    [Year(2020)]
    [Day(13)]
    [Test("939\n7,13,x,x,59,x,31,19", "295", "1068781")]
    //[Test("\n17,x,13,19", null, "3417")]
    //[Test("\n67,7,59,61", null, "754018")]
    //[Test("\n67,x,7,59,61", null, "779210")]
    //[Test("\n67,7,x,59,61", null, "1261476")]
    //[Test("\n1789,37,47,1889", null, "1202161486")]
    public class Day13: Puzzle
    {
        private Tuple<int, List<int>> ParseInput(string input)
        {
            var inputLines = input.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var arrivalTime = int.Parse(inputLines[0]);
            var busTimes = inputLines[1].Split(',').Where(s => s != "x").Select(s => int.Parse(s)).ToList();

            return new Tuple<int, List<int>>(arrivalTime, busTimes);
        }
        private List<Tuple<long, long>> ParseBusPattern(string input)
        {
            var busPattern = new List<Tuple<long, long>>();
            var inputList = input.Split('\n')[1].Split(',');

            for(var counter = 0; counter < inputList.Length; ++counter)
            {
                if (inputList[counter] == "x") continue;
                var busId = long.Parse(inputList[counter]);
                var busDelay = (busId - counter) % busId;

                if (busDelay < 0) busDelay += busId * (long)Math.Ceiling((Math.Abs(busDelay) / (double)busId));

                busPattern.Add(new Tuple<long, long>(busId, busDelay));
            }

            return busPattern;
        }

        private Tuple<long, long> SolveBezoutIdentity(long n, long m) // Nn + Mm = 1
        {
            var swap = false;

            if (n < m)
            {
                var temp = n;
                n = m;
                m = temp;
                swap = true;
            }

            var pairFound = false;
            var N = 0L;
            var M = 0L;

            var hcf = MathHelper.GetHighestCommonFactor(n, m);

            while (!pairFound)
            {
                var nN = n * N;

                if ((nN - hcf.Value) % m == 0)
                {
                    M = -((nN - hcf.Value) / m);
                    pairFound = true;
                }
                else if ((nN + hcf.Value) % m == 0)
                {
                    M = (nN + hcf.Value) / m;
                    N = -N;
                    pairFound = true;
                }
                else ++N;
            }

            return swap ? new Tuple<long, long>(M, N) : new Tuple<long, long>(N, M);
        }

        private long GetMatchingDivisor(long n1, long a1, long n2, long a2)
        {
            var bezoutPair = SolveBezoutIdentity(n1, n2);

            var divisorOffset = ((BigInteger)n1 * bezoutPair.Item1 * a2) + ((BigInteger)n2 * bezoutPair.Item2 * a1);
            var product = n1 * n2;
            var mutliplier = new BigInteger(0);

            if (divisorOffset < 0) mutliplier = ((-divisorOffset / product) + 1);
            else if (divisorOffset > product) mutliplier = -(divisorOffset / product);

            return (long)((mutliplier * product) + divisorOffset);
        }

        protected override string Part1(string input)
        {
            var parsedInput = ParseInput(input);
            parsedInput.Item2.Sort((a, b) => (a - (parsedInput.Item1 % a)).CompareTo(b - (parsedInput.Item1 % b)));

            return (parsedInput.Item2[0] * (parsedInput.Item2[0] - (parsedInput.Item1 % parsedInput.Item2[0]))).ToString();
        }
        protected override string Part2(string input)
        {
            var moduloRules = ParseBusPattern(input);

            while (moduloRules.Count > 1)
            {
                var newRules = new List<Tuple<long, long>>();

                for(var counter = 0; counter < moduloRules.Count - 1; counter += 2)
                {
                    var divisor = GetMatchingDivisor(moduloRules[counter].Item1, moduloRules[counter].Item2, moduloRules[counter + 1].Item1, moduloRules[counter + 1].Item2);
                    var product = moduloRules[counter].Item1 * moduloRules[counter + 1].Item1;

                    newRules.Add(new Tuple<long, long>(product, divisor));
                }

                if (moduloRules.Count % 2 != 0) newRules.Add(moduloRules.Last());

                moduloRules = newRules;
            }


            return moduloRules.Single().Item2.ToString();
        }
    }
}

