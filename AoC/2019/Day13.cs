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
    [Day(13)]
    [Test("input",null, null)]
    [Test("input", null, null)]
    [Test("input", null, null)]
    [Test("input", null, null)]
    public class Day13 : _2019Puzzle
    {
        protected override string Part1(string input)
        {
            var intcodeComputer = GetIntcodeComputer(input);
            var intcodeResult = intcodeComputer.RunProgram(new IntcodeProgramInput()).GetAwaiter().GetResult();
            var allOutputs = intcodeResult.GetOutputs();
            var blockCounter = 0;

            for (var counter = 2; counter < allOutputs.Count; counter += 3) blockCounter += allOutputs[counter] == 2 ? 1 : 0;

            return blockCounter.ToString();
        }

        protected override string Part2(string input)
        {
            while (true)
            {
                var intcodeComputer = GetIntcodeComputer(input);
                intcodeComputer.RunProgram(new IntcodeProgramInput(mem => mem[0] = 2, new long[] { 0 }));

                var gameState = new Dictionary<string, int>();
                var maxX = 0;
                var maxY = 0;

                var score = 0;

                var lastBallX = -1;

                while (!intcodeComputer.Exited)
                {
                    while (true)
                    {
                        var outputGroup = new int[3] { -2, -2, -2 };

                        for (var outCounter = 0; outCounter < 3; ++outCounter)
                        {
                            var thisOut = intcodeComputer.GetNextOutput();   
                            outputGroup[outCounter] = thisOut.HasValue ? (int)thisOut.Value : -2;
                        }

                        if (outputGroup.Any(i => i == -2)) break;

                        if (outputGroup[0] > maxX) maxX = outputGroup[0];
                        if (outputGroup[1] > maxY) maxY = outputGroup[1];

                        var tileKey = outputGroup[0].ToString() + "," + outputGroup[1].ToString();
                        if (!gameState.ContainsKey(tileKey)) gameState.Add(tileKey, 0);
                        gameState[tileKey] = outputGroup[2];
                    }

                    if (gameState.ContainsKey("-1,0")) score = gameState["-1,0"];
                    if (gameState.Count == 0) continue;

                    Console.Clear();

                    Console.WriteLine("Score: {0}", gameState.ContainsKey("-1,0") ? gameState["-1,0"].ToString() : "null");
                    Console.WriteLine();

                    for (var y = 0; y <= maxY; ++y)
                    {
                        for (var x = 0; x <= maxX; ++x)
                        {
                            var tileKey = x.ToString() + "," + y.ToString();
                            if (!gameState.ContainsKey(tileKey) || gameState[tileKey] == 0) Console.Write(" ");
                            else
                            {
                                switch (gameState[tileKey])
                                {
                                    case 1:
                                        Console.Write("█");
                                        break;
                                    case 2:
                                        Console.Write("=");
                                        break;
                                    case 3:
                                        Console.Write("#");
                                        break;
                                    case 4:
                                        Console.Write("O");
                                        break;
                                    default:
                                        Console.Write("?");
                                        break;
                                }
                            }
                        }
                        Console.WriteLine();
                    }

                    var ballX = int.Parse(gameState.First(kvp => kvp.Value == 4).Key.Split(',')[0]);
                    var paddleX = int.Parse(gameState.First(kvp => kvp.Value == 3).Key.Split(',')[0]);

                    if (gameState.Count(kvp => kvp.Value == 2) == 0) intcodeComputer.AddInput(0);
                    else if (lastBallX == -1 || lastBallX == ballX) intcodeComputer.AddInput(paddleX == ballX ? 0 : paddleX < ballX ? 1 : -1);
                    else if (lastBallX > ballX) intcodeComputer.AddInput(paddleX < ballX ? 1 : -1);
                    else intcodeComputer.AddInput(paddleX > ballX ? -1 : 1);

                    lastBallX = ballX;

                    Thread.Sleep(500);
                }

                Console.WriteLine();

                if (gameState.Count(kvp => kvp.Value == 2) == 0)
                {
                    Console.WriteLine("A WINNER IS YOU!");
                    return score.ToString();
                }
                else
                {
                    Console.WriteLine("GAME OVER");
                    Console.WriteLine("press any key to try again");
                    Console.ReadKey();
                }
            }
        }
    }
}
