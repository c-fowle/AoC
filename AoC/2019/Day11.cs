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
    [Day(11)]
    [Test("input",null, null)]
    [Test("input", null, null)]
    [Test("input", null, null)]
    [Test("input", null, null)]
    public class Day11 : _2019Puzzle
    {
        private int[] ParseInput(string input) => input.Split('\n').Select(s => int.Parse(s)).ToArray();

        private Dictionary<string, bool> PaintHull(string input, long initialTileColour)
        {
            var robotDirection = 0;
            var robotPosition = new int[] { 0, 0 };
            var allTiles = new Dictionary<string, bool>();

            var intcodeComputer = GetIntcodeComputer(input);

            intcodeComputer.RunProgram(new IntcodeProgramInput(inputs: new long[] { initialTileColour }));

            while (!intcodeComputer.Exited)
            {
                if (!intcodeComputer.OutputReady()) continue;
                var colour = intcodeComputer.GetLastOutput() == 1;

                while (!intcodeComputer.OutputReady()) if (intcodeComputer.Exited) break;
                if (!intcodeComputer.OutputReady() && intcodeComputer.Exited) break;

                var turnDirection = intcodeComputer.GetLastOutput() == 1;
                var posString = robotPosition[0].ToString() + "," + robotPosition[1].ToString();

                if (!allTiles.ContainsKey(posString)) allTiles.Add(posString, colour);
                else allTiles[posString] = colour;

                robotDirection = (robotDirection + (turnDirection ? 1 : -1) + 4) % 4;

                switch (robotDirection)
                {
                    case 0:
                        --robotPosition[1];
                        break;
                    case 1:
                        ++robotPosition[0];
                        break;
                    case 2:
                        ++robotPosition[1];
                        break;
                    case 3:
                        --robotPosition[0];
                        break;
                }

                posString = robotPosition[0].ToString() + "," + robotPosition[1].ToString();

                intcodeComputer.AddInput(allTiles.ContainsKey(posString) && allTiles[posString] ? 1 : 0);
            }

            return allTiles;
        }

        protected override string Part1(string input) => PaintHull(input, 0).Count().ToString();

        protected override string Part2(string input)
        {
            var allTiles = PaintHull(input, 1);

            var maxX = allTiles.Keys.Max(k => int.Parse(k.Split(',')[0]));
            var minX = allTiles.Keys.Min(k => int.Parse(k.Split(',')[0]));

            var maxY = allTiles.Keys.Max(k => int.Parse(k.Split(',')[1]));
            var minY = allTiles.Keys.Min(k => int.Parse(k.Split(',')[1]));

            Console.WriteLine();
            Console.WriteLine("SHIP HULL:");
            Console.WriteLine();

            for (var y = minY; y <= maxY; ++y)
            {
                for (var x = minX; x <= maxX; ++x)
                {
                    var posString = x.ToString() + "," + y.ToString();
                    if (allTiles.ContainsKey(posString) && allTiles[posString]) Console.Write("█");
                    else Console.Write(" ");
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("What do you see?");
            return Console.ReadLine();
        }
    }
}
