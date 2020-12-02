using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AoC.Common;
using AoC.Common.Attributes;
using AoC.Common.ExtensionMethods;
using AoC.Common.Helpers;

using AoC._2019.Classes;
using AoC._2019.Enums;

namespace AoC._2019
{
    public class Moon
    {
        public int ID { get; }

        private List<int[]>[] History { get; }
        public int[] CoordinateLoopLengths { get; }
        private long? _LoopLength { get; set; }

        private int[] InitialPosition { get; }
           
        public bool LoopExists { get => CoordinateLoopLengths.All(i => i != -1); }
        public long LoopLength
        {
            get
            {
                if (!_LoopLength.HasValue) _LoopLength = MathHelper.GetLowestCommonMultiple(CoordinateLoopLengths[0], MathHelper.GetLowestCommonMultiple(CoordinateLoopLengths[1], CoordinateLoopLengths[2]));
                return _LoopLength.Value;
            }
        }

        public Moon(int id, int x, int y, int z)
        {
            ID = id;

            History = new List<int[]>[] { new List<int[]>(), new List<int[]>(), new List<int[]>() };
            CoordinateLoopLengths = new int[] { -1, -1, -1 };
            InitialPosition = new int[] { x, y, z };

            History[0].Add(new int[] { x, 0 });
            History[1].Add(new int[] { y, 0 });
            History[2].Add(new int[] { z, 0 });
        }
        public int[] GetStateAtStep(long stepNo)
        {
            var state = new int[6];

            for (var i = 0; i < 3; ++i)
            {
                if (History[i].Count > stepNo)
                {
                    state[i] = History[i][(int)stepNo][0];
                    state[i + 3] = History[i][(int)stepNo][1];
                    continue;
                }
                if (CoordinateLoopLengths[i] == -1) throw new Exception("No position for requested step and no loop in coordinate");

                var loopPosition = (int)(stepNo % CoordinateLoopLengths[i]);
                state[i] = History[i][loopPosition][0];
                state[i + 3] = History[i][loopPosition][1];
            }

            return state;
        }
        public void UpdatePosition(int[] acceleration)
        {
            if (LoopExists) return;

            for (var i = 0; i < 3; ++i)
            {
                if (CoordinateLoopLengths[i] != -1) continue;

                var previousState = History[i].Last();
                var newState = new int[2];

                newState[1] = previousState[1] + acceleration[i];
                newState[0] = previousState[0] + newState[1];

                if (newState[0] == InitialPosition[i] && newState[1] == 0)
                {
                    var occurances = History[i].Where(h => h.SequenceEqual(newState)).ToList();
                    if (occurances.Count() % 2 == 0)
                    {
                        var loopRestart = History[i].IndexOf(occurances[occurances.Count() / 2]);

                        if (History[i].Count >= loopRestart * 2)
                        {
                            var loopFound = true;
                            for (var j = 0; j < loopRestart; ++j)
                            {
                                try
                                {
                                    loopFound &= History[i][j].SequenceEqual(History[i][loopRestart + j]);
                                    if (!loopFound) break;
                                }
                                catch (Exception ex)
                                {
                                    var a = ex;
                                }
                            }
                            if (loopFound) CoordinateLoopLengths[i] = loopRestart;
                        }
                    }
                }

                History[i].Add(newState);
            }
        }
    }

    [Year(2019)]
    [Day(12)]
    [Test("<x=-1, y=0, z=2>\n<x=2, y=-10, z=-7>\n<x=4, y=-8, z=8>\n<x=3, y=5, z=-1>", null, "2772")]
    [Test("<x=-8, y=-10, z=0>\n<x=5, y=5, z=10>\n<x=2, y=-7, z=3>\n<x=9, y=-8, z=-3>", null, "4686774924")]
    public class Day12 : _2019Puzzle
    {
        private int[] AccelerationByRank { get; set; }

        private Dictionary<int, Moon> ParseInput(string input)
        {
            var parsingRegex = new Regex("^<x=(?<X>-?\\d*), y=(?<Y>-?\\d*), z=(?<Z>-?\\d*)>$");
            var lines = input.Split('\n');
            var moonDictionary = new Dictionary<int, Moon>();

            for (var counter = 0; counter < lines.Length; ++counter)
            {
                var match = parsingRegex.Match(lines[counter]);
                if (!match.Success) continue;
                moonDictionary.Add(counter, new Moon(counter, int.Parse(match.Groups["X"].Value), int.Parse(match.Groups["Y"].Value), int.Parse(match.Groups["Z"].Value)));
            }

            return moonDictionary;
        }

        private Dictionary<int, int[]> GetAccelerations(long lastStep, Dictionary<int, Moon> allMoons)
        {
            var positions = allMoons.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.GetStateAtStep(lastStep));
            var accelerations = allMoons.ToDictionary(kvp => kvp.Key, kvp => new int[] { 0, 0, 0 });

            var keys = positions.Keys.ToList();

            for (var i = 0; i < keys.Count; ++i)
            {
                for (var j = (i + 1); j < keys.Count; ++j)
                {
                    for (var coordinate = 0; coordinate < 3; ++coordinate)
                    {
                        if (positions[i][coordinate] < positions[j][coordinate])
                        {
                            accelerations[i][coordinate]++;
                            accelerations[j][coordinate]--;
                        }
                        else if (positions[i][coordinate] > positions[j][coordinate])
                        {
                            accelerations[i][coordinate]--;
                            accelerations[j][coordinate]++;
                        }
                    }
                }
            }

            return accelerations;
        }

        private void UpdateVelocities(int[][] positions, int[][] velocities)
        {
            for (var moon1 = 0; moon1 < positions.Length; ++moon1)
            {
                var acceleration = new int[] { 0, 0, 0 };

                for (var moon2 = 0; moon2 < positions.Length; ++moon2)
                {
                    if (moon2 == moon1) continue;
                    for (var coord = 0; coord < 3; ++coord) acceleration[coord] += positions[moon2][coord] > positions[moon1][coord] ? 1 : positions[moon2][coord] == positions[moon1][coord] ? 0 : -1;
                }

                for (var coord = 0; coord < 3; ++coord) velocities[moon1][coord] += acceleration[coord];
            }
        }


        private string GetState(int[][] positions, int[][] velocities)
        {
            var objectStates = new List<string>();
            for (var counter = 0; counter < positions.Length; ++counter)
                objectStates.Add(
                    String.Format("<x={0}, y={1}, z={2}, xv={3}, yv={4}, zv={5}>",
                        positions[counter][0],        
                        positions[counter][1],
                        positions[counter][2],
                        velocities[counter][0],
                        velocities[counter][1],
                        velocities[counter][2]));
            return String.Join(";", objectStates);
        }

        private void UpdatePositions(int[][] positions, int[][] velocities)
        {
            for (var counter = 0; counter < positions.Length; ++counter) for (var coord = 0; coord < 3; ++coord) positions[counter][coord] += velocities[counter][coord];
        }

        protected override string Part1(string input)
        {
            var moons = ParseInput(input);

            AccelerationByRank = new int[moons.Count];
            for (var rankCounter = 0; rankCounter < AccelerationByRank.Length; ++rankCounter) AccelerationByRank[rankCounter] = (AccelerationByRank.Length - 1) - (2 * rankCounter);

            for (var stepCount = 0; stepCount < 1000; ++stepCount)
            {
                if (moons.All(kvp => kvp.Value.LoopExists)) break;
                var currentAccelerations = GetAccelerations(stepCount, moons);
                moons.ForEach(kvp => kvp.Value.UpdatePosition(currentAccelerations[kvp.Key]));
            }

            var totalEnergy = moons.Select(kvp =>
            {
                var state = kvp.Value.GetStateAtStep(1000);
                return (Math.Abs(state[0]) + Math.Abs(state[1]) + Math.Abs(state[2])) * (Math.Abs(state[3]) + Math.Abs(state[4]) + Math.Abs(state[5]));
            }).ToArray();

            return totalEnergy.Sum().ToString();
        }

        protected override string Part2(string input)
        {
            var moons = ParseInput(input);

            AccelerationByRank = new int[moons.Count];
            for (var rankCounter = 0; rankCounter < AccelerationByRank.Length; ++rankCounter) AccelerationByRank[rankCounter] = (AccelerationByRank.Length - 1) - (2 * rankCounter);

            var stepNumber = 0L;

            while(true)
            {
                if (moons.All(kvp => kvp.Value.LoopExists)) break;

                var currentAccelerations = GetAccelerations(stepNumber++, moons);
                moons.ForEach(kvp => kvp.Value.UpdatePosition(currentAccelerations[kvp.Key]));             
            }

            var moonKeys = moons.Keys.ToList();
            var loopDuration = (long)moons[moonKeys[0]].LoopLength;

            for (var i = 1; i < moonKeys.Count; ++i)
            {
                loopDuration = MathHelper.GetLowestCommonMultiple(loopDuration, moons[moonKeys[i]].LoopLength);
            }

            Console.WriteLine(loopDuration.ToString());

            return loopDuration.ToString();
        }
    }
}
