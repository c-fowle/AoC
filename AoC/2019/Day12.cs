using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AoC.Common;
using AoC.Common.Attributes;
using AoC.Common.ExtensionMethods;

using AoC._2019.Classes;
using AoC._2019.Enums;

namespace AoC._2019
{
    public class Moon
    {
        public int ID { get; }

        private List<int[]> History { get; }
        private int LoopStart { get; set; }
        private int LoopEnd { get; set; }

        public bool LoopExists { get => LoopStart != -1 && LoopEnd != -1; }

        public Moon(int id, int x, int y, int z)
        {
            ID = id;

            History = new List<int[]>();

            var initialState = new int[] { x, y, z, 0, 0, 0 };
            History.Add(initialState);

            LoopStart = -1;
            LoopEnd = -1;
        }
        public int[] GetStateAtStep(long stepNo)
        {
            if (History.Count > stepNo) return History[(int)stepNo];
            if (!LoopExists) return null;

            var loopPosition = (int)((stepNo - LoopStart) % (LoopEnd - LoopStart));
            if (History.Count <= loopPosition) return null;

            return History[LoopStart + loopPosition].CloneAsList().ToArray();
        }
        public void UpdatePosition(int[] acceleration)
        {
            if (LoopExists) return;

            var previousState = History.Last();
            var newState = new int[6];
            
            newState[3] = previousState[3] + acceleration[0];
            newState[4] = previousState[4] + acceleration[1];
            newState[5] = previousState[5] + acceleration[2];

            newState[0] = previousState[0] + newState[3];
            newState[1] = previousState[1] + newState[4];
            newState[2] = previousState[2] + newState[5];

            var matches = History.Where(old => old.SequenceEqual(newState));

            if (matches.Count() < 2) History.Add(newState);
            else
            {
                var first = History.FindIndex(i => i.SequenceEqual(newState));
                var second = History.FindLastIndex(i => i.SequenceEqual(newState));

                var counter = 0;
                var loops = true;

                while (counter < (second - first) && loops)
                {
                    if (History.Count <= (second + counter)) loops = false;
                    else loops &= History[first + counter].SequenceEqual(History[second + counter]);
                    ++counter;
                }

                if (loops)
                {
                    LoopStart = first;
                    LoopEnd = second;
                }
                else History.Add(newState);
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

            for(var coordinate = 0; coordinate < 3; ++coordinate)
            {
                var orderedByPosition = positions.ToDictionary(kvp => kvp.Key, kvp => kvp.Value[coordinate]).OrderBy(kvp => kvp.Value);

                if (orderedByPosition == null) throw new Exception();

                var first = orderedByPosition.First();

                var lastScore = first.Value;
                var ids = new List<int> { first.Key };
                var ranks = new List<int> { 0 };

                for (var counter = 1; counter < orderedByPosition.Count(); ++counter)
                {
                    if (lastScore == orderedByPosition.ElementAt(counter).Value)
                    {
                        ids.Add(orderedByPosition.ElementAt(counter).Key);
                        ranks.Add(counter);
                    }
                    else
                    {
                        ids.ForEach(id => accelerations[id][coordinate] = (ranks.Sum(r => AccelerationByRank[r]) / ids.Count));

                        lastScore = orderedByPosition.ElementAt(counter).Value;
                        ids = new List<int> { orderedByPosition.ElementAt(counter).Key };
                        ranks = new List<int> { counter };
                    }
                }

                ids.ForEach(id => accelerations[id][coordinate] = (ranks.Sum(r => AccelerationByRank[r]) / ids.Count));
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
                if (!moons.All(kvp => kvp.Value.LoopExists))
                {
                    var currentAccelerations = GetAccelerations(stepNumber++, moons);
                    moons.ForEach(kvp => kvp.Value.UpdatePosition(currentAccelerations[kvp.Key]));
                }

                for (var backCount = 0L; backCount < stepNumber; ++backCount)
                {
                    if (moons.All(kvp => kvp.Value.GetStateAtStep(backCount).SequenceEqual(kvp.Value.GetStateAtStep(stepNumber)))) return stepNumber.ToString();
                }

            }
        }
    }
}
