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
    class Point
    {
        public static bool operator ==(Point p1, Point p2)
        {
            if (p1 is null && p2 is null) return true;
            if (p1 is null ^ p2 is null) return false;
            return p1.X == p2.X && p1.Y == p2.Y;
        }
        public static bool operator !=(Point p1, Point p2)
        {
            if (p1 is null && p2 is null) return false;
            if (p1 is null ^ p2 is null) return true;
            return p1.X != p2.X || p1.Y != p2.Y;
        }

        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object o)
        {
            if (o is null) return false;
            if (!typeof(Point).IsInstanceOfType(o)) return false;
            var castObject = (Point)o;
            return X == castObject.X && Y == castObject.Y;
        }
        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + X.GetHashCode();
            hash = (hash * 7) + Y.GetHashCode();
            return hash;
        }

        public Point[] GetNeighbours()
        {
            return new[] {
                new Point(X, Y - 1),
                new Point(X, Y + 1),
                new Point(X - 1, Y),
                new Point(X + 1, Y)
            };
        }
    }

    [Year(2019)]
    [Day(15)]
    [Test("input",null, null)]
    [Test("input", null, null)]
    [Test("input", null, null)]
    [Test("input", null, null)]
    public class Day15 : _2019Puzzle
    {
        private Point GetTargetPoint(Point currentPosition, int direction)
        {
            switch(direction)
            {
                case 1:
                    return new Point(currentPosition.X, currentPosition.Y - 1);
                case 2:
                    return new Point(currentPosition.X, currentPosition.Y + 1);
                case 3:
                    return new Point(currentPosition.X - 1, currentPosition.Y);
                case 4:
                    return new Point(currentPosition.X + 1, currentPosition.Y);
                default:
                    throw new Exception("Unexpected droid direction");
            }
        }

        private int TurnLeft(int currentDirection)
        {
            switch(currentDirection)
            {
                case 1: // North
                    return 3;
                case 2: // South
                    return 4;
                case 3: // West
                    return 2;
                case 4: // East
                    return 1;
                default:
                    throw new Exception("Unexpected droid direction");
            }
        }

        private int TurnRight(int currentDirection)
        {
            switch (currentDirection)
            {
                case 1: // North
                    return 4;
                case 2: // South
                    return 3;
                case 3: // West
                    return 1;
                case 4: // East
                    return 2;
                default:
                    throw new Exception("Unexpected droid direction");
            }
        }

        private bool OpenTiles(Dictionary<Point, bool> attemptedTiles)
        {
            var walkable = attemptedTiles.Where(kvp => kvp.Value);

            foreach (var kvp in walkable) if (kvp.Key.GetNeighbours().Any(n => !attemptedTiles.ContainsKey(n))) return true;
            return false;
        }

        private Tuple<Dictionary<Point, bool>, Point> GetMap(string input)
        {
            var intcodeComputer = GetIntcodeComputer(input);
            intcodeComputer.RunProgram(new IntcodeProgramInput());

            var droidFacing = 1;
            var droidPosition = new Point(0, 0);
            var tiles = new Dictionary<Point, bool>();
            tiles.Add(droidPosition, true);

            var oxygenSystemPosition = default(Point);

            while (OpenTiles(tiles))
            {
                var targetPosition = GetTargetPoint(droidPosition, droidFacing);

                var leftTurn = TurnLeft(droidFacing);
                intcodeComputer.AddInput(leftTurn);

                var droidResponse = default(long?);
                while (droidResponse == null) droidResponse = intcodeComputer.GetNextOutput();

                switch (droidResponse)
                {
                    case 0:
                        droidFacing = TurnRight(droidFacing);
                        if (!tiles.ContainsKey(targetPosition)) tiles.Add(targetPosition, false);
                        break;
                    case 1:
                    case 2:
                        droidPosition = targetPosition;
                        droidFacing = leftTurn;
                        if (!tiles.ContainsKey(targetPosition)) tiles.Add(targetPosition, true);
                        if (droidResponse == 2) oxygenSystemPosition = targetPosition;
                        break;
                    default:
                        throw new Exception("Unexpected droid response");
                }
            }

            return new Tuple<Dictionary<Point, bool>, Point>(tiles, oxygenSystemPosition);
        }

        private void RouteFindNextStep(Dictionary<Point, bool> tiles, Dictionary<Point, int> openSet, Dictionary<Point, int> closedSet)
        {
            var currentTile = openSet.OrderBy(kvp => kvp.Value).First().Key;
            var neighbourScore = openSet[currentTile] + 1;
            foreach (var p in currentTile.GetNeighbours())
            {
                if (!tiles.ContainsKey(p)) continue;
                else if (!tiles[p]) continue;

                if (closedSet.ContainsKey(p)) continue;
                if (openSet.ContainsKey(p))
                {
                    if (openSet[p] < neighbourScore) continue;
                    else openSet[p] = neighbourScore;
                }
                else openSet.Add(p, neighbourScore);
            }

            closedSet.Add(currentTile, openSet[currentTile]);
            openSet.Remove(currentTile);
        }

        protected override string Part1(string input)
        {
            var mapResult = GetMap(input);

            var tiles = mapResult.Item1;
            var oxygenSystemPosition = mapResult.Item2;

            // A* from 0,0 to oxygenSystemPosition

            var openSet = new Dictionary<Point, int>();
            var closedSet = new Dictionary<Point, int>();

            openSet.Add(new Point(0, 0), 0);

            while (!closedSet.ContainsKey(oxygenSystemPosition)) RouteFindNextStep(tiles, openSet, closedSet);

            return closedSet[oxygenSystemPosition].ToString();
        }

        protected override string Part2(string input)
        {
            var mapResult = GetMap(input);

            var tiles = mapResult.Item1;
            var oxygenSystemPosition = mapResult.Item2;

            var openSet = new Dictionary<Point, int>();
            var closedSet = new Dictionary<Point, int>();

            openSet.Add(oxygenSystemPosition, 0);

            while (openSet.Count > 0) RouteFindNextStep(tiles, openSet, closedSet);

            return closedSet.Select(kvp => kvp.Value).Max().ToString();
        }
    }
}
