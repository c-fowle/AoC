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
    [Day(3)]
    [Test("U10\nR2,U3,L4,U2,R4\nL3,U5,R5", "5", "29")] // Additional test for arbitrary number of wires
    [Test("R8,U5,L5,D3\nU7,R6,D4,L4", "6", "30")]
    [Test("R75,D30,R83,U83,L12,D49,R71,U7,L72\nU62,R66,U55,R34,D71,R55,D58,R83", "159", "610")]
    [Test("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51\nU98,R91,D20,R16,D67,R40,U7,R15,U6,R7", "135", "410")]
    public class Day03 : _2019Puzzle
    {
        private List<List<Tuple<char, int>>> ParseInput(string input) => input.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(t => new Tuple<char, int>(t.Trim()[0], int.Parse(t.Trim().Substring(1)))).ToList()).ToList();

        private Dictionary<string, int>[] GetWirePositions(string input)
        {
            var wireMoves = ParseInput(input);
            var finalState = new Dictionary<string, int>[wireMoves.Count];

            var stepFunc = new Func<char, int[], int[]>((dir, pos) =>
            {
                switch (dir)
                {
                    case 'R':
                        return new[] { pos[0] + 1, pos[1] };
                    case 'L':
                        return new[] { pos[0] - 1, pos[1] };
                    case 'D':
                        return new[] { pos[0], pos[1] + 1 };
                    case 'U':
                        return new[] { pos[0], pos[1] - 1 };
                    default:
                        throw new Exception("Unrecognised direction");
                }
            });

            for (var i = 0; i < wireMoves.Count; ++i)
            {
                var stepCount = 0;
                var currentPosition = new int[] { 0, 0 };

                finalState[i] = new Dictionary<string, int> { { "0,0", 0 } };

                wireMoves[i].ForEach(move =>
                {
                    for (var j = 0; j < move.Item2; ++j)
                    {
                        currentPosition = stepFunc(move.Item1, currentPosition);
                        ++stepCount;

                        var posKey = String.Join(",", currentPosition.Select(p => p.ToString()));

                        if (finalState[i].ContainsKey(posKey)) continue;
                        finalState[i].Add(posKey, stepCount);
                    }
                });
            }

            return finalState;
        }

        protected override string Part1(string input)
        {
            var wireState = GetWirePositions(input);
            if (wireState.Length < 2) throw new Exception("Insufficient wires in input");
            return wireState[0].Keys.Where(k => k != "0,0" && wireState.All(dict => dict.ContainsKey(k))).Select(k => k.Split(new[] { ',' }).Select(coord => Math.Abs(int.Parse(coord))).Sum()).Min().ToString();
        }

        protected override string Part2(string input)
        {
            var wireState = GetWirePositions(input);
            if (wireState.Length < 2) throw new Exception("Insufficient wires in input");
            return wireState[0].Keys.Where(k => k != "0,0" && wireState.All(dict => dict.ContainsKey(k))).Select(k => wireState.Select(dict => dict[k]).Sum()).Min().ToString();
        }
    }
}
