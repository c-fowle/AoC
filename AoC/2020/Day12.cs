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
    [Day(12)]
    [Test("F10\nN3\nF7\nR90\nF11", "25", "286")]
    public class Day12: Puzzle
    {
        private List<Tuple<char, int>> ParseInput(string input) => input.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(s => new Tuple<char, int>(s.First(), int.Parse(s.Substring(1)))).ToList();

        protected override string Part1(string input)
        {
            var parsedInput = ParseInput(input);

            var bearing = 90D;
            var x = 0;
            var y = 0;

            // x change = sin(bearing)
            // y change = -cos(bearing)

            for(var i = 0; i < parsedInput.Count; ++i)
            {
                switch (parsedInput[i].Item1)
                {
                    case 'N':
                        y -= parsedInput[i].Item2;
                        break;
                    case 'S':
                        y += parsedInput[i].Item2;
                        break;
                    case 'E':
                        x += parsedInput[i].Item2;
                        break;
                    case 'W':
                        x -= parsedInput[i].Item2;
                        break;
                    case 'L':
                        bearing = ((bearing + 360) - parsedInput[i].Item2) % 360;
                        break;
                    case 'R':
                        bearing = (bearing + parsedInput[i].Item2) % 360;
                        break;
                    case 'F':
                        x += (int)Math.Round((double)parsedInput[i].Item2 * Math.Sin((bearing / 180) * Math.PI));
                        y -= (int)Math.Round((double)parsedInput[i].Item2 * Math.Cos((bearing / 180) * Math.PI));
                        break;
                    default:
                        throw new Exception("Bad");

                }
            }

            return (Math.Abs(x) + Math.Abs(y)).ToString();
        }
        protected override string Part2(string input)
        {
            var parsedInput = ParseInput(input);

            var x = 0;
            var y = 0;

            var waypointX = 10;
            var waypointY = -1;

            int tempX;
            int tempY;

            double rotationAngle;

            // x change = sin(bearing)
            // y change = -cos(bearing)

            for (var i = 0; i < parsedInput.Count; ++i)
            {
                switch (parsedInput[i].Item1)
                {
                    case 'N':
                        waypointY -= parsedInput[i].Item2;
                        break;
                    case 'S':
                        waypointY += parsedInput[i].Item2;
                        break;
                    case 'E':
                        waypointX += parsedInput[i].Item2;
                        break;
                    case 'W':
                        waypointX -= parsedInput[i].Item2;
                        break;
                    case 'L':
                        rotationAngle = (parsedInput[i].Item2 / 180D) * Math.PI * (-1);

                        tempX = (int)Math.Round(((double)waypointX * Math.Cos(rotationAngle)) - ((double)waypointY * Math.Sin(rotationAngle)));
                        tempY = (int)Math.Round(((double)waypointX * Math.Sin(rotationAngle)) + ((double)waypointY * Math.Cos(rotationAngle)));
                        waypointX = tempX;
                        waypointY = tempY;

                        break;
                    case 'R':
                        rotationAngle = (parsedInput[i].Item2 / 180D) * Math.PI;

                        tempX = (int)Math.Round(((double)waypointX * Math.Cos(rotationAngle)) - ((double)waypointY * Math.Sin(rotationAngle)));
                        tempY = (int)Math.Round(((double)waypointX * Math.Sin(rotationAngle)) + ((double)waypointY * Math.Cos(rotationAngle)));
                        waypointX = tempX;
                        waypointY = tempY;

                        break;
                    case 'F':
                        x += (waypointX * parsedInput[i].Item2);
                        y += (waypointY * parsedInput[i].Item2);
                        break;
                    default:
                        throw new Exception("Bad");

                }
            }

            return (Math.Abs(x) + Math.Abs(y)).ToString();
        }
    }
}

