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
    [Day(11)]
    [Test("L.LL.LL.LL\nLLLLLLL.LL\nL.L.L..L..\nLLLL.LL.LL\nL.LL.LL.LL\nL.LLLLL.LL\n..L.L.....\nLLLLLLLLLL\nL.LLLLLL.L\nL.LLLLL.LL", "37", "26")]
    public class Day11: Puzzle
    {
        private int[][] ParseInput(string input) => input.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Select(c => c == '.' ? 0 : 1).ToArray()).ToArray();

        private int GetNextState(int row, int column, int[][] seatState, int maxDistance = 1, int maxOccupiedNeighbours = 4)
        {
            var neighbourStates = new List<int>();
            
            for (var deltaR = -1; deltaR < 2; ++deltaR)
            {
                for (var deltaC = -1; deltaC < 2; ++deltaC)
                {
                    if (deltaR == 0 && deltaC == 0) continue;

                    var distance = 1;

                    while (distance <= maxDistance)
                    {
                        if (row + (deltaR * distance) < 0 || row + (deltaR * distance) >= seatState.GetLength(0)) break;
                        if (column + (deltaC * distance) < 0 || column + (deltaC * distance) >= seatState[0].Length) break;

                        if (seatState[row + (deltaR * distance)][column + (deltaC * distance)] == 0) distance++;
                        else
                        {
                            neighbourStates.Add(seatState[row + (deltaR * distance)][column + (deltaC * distance)]);
                            break;
                        }
                    }
                }
            }

            if (seatState[row][column] == 1 && neighbourStates.Count(i => i == 2) == 0) return 2;
            if (seatState[row][column] == 2 && neighbourStates.Count(i => i == 2) >= maxOccupiedNeighbours) return 1;

            return seatState[row][column];
        }

        protected override string Part1(string input)
        {
            var seatMap = ParseInput(input);

            while (true)
            {
                var newSeatmap = new int[seatMap.GetLength(0)][];
                var seatChange = false;

                for (var row = 0; row < seatMap.GetLength(0); ++row)
                {
                    newSeatmap[row] = new int[seatMap[row].Length];
                    for (var column = 0; column < seatMap[row].Length; ++column)
                    {
                        newSeatmap[row][column] = GetNextState(row, column, seatMap);
                        seatChange |= (newSeatmap[row][column] != seatMap[row][column]);
                    }
                }

                seatMap = newSeatmap;
                if (!seatChange) break;
            }

            return seatMap.Select(row => row.Count(i => i == 2)).Sum().ToString();
        }
        protected override string Part2(string input)
        {
            var seatMap = ParseInput(input);
            var mapSize = seatMap.GetLength(0);

            while (true)
            {
                var newSeatmap = new int[seatMap.GetLength(0)][];
                var seatChange = false;

                for (var row = 0; row < seatMap.GetLength(0); ++row)
                {
                    newSeatmap[row] = new int[seatMap[row].Length];
                    for (var column = 0; column < seatMap[row].Length; ++column)
                    {
                        newSeatmap[row][column] = GetNextState(row, column, seatMap, mapSize * 2, 5);
                        seatChange |= (newSeatmap[row][column] != seatMap[row][column]);
                    }
                }

                seatMap = newSeatmap;
                if (!seatChange) break;
            }

            return seatMap.Select(row => row.Count(i => i == 2)).Sum().ToString();
        }
    }
}

