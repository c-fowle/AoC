using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Common;
using AoC.Common.Attributes;
using AoC.Common.ExtensionMethods;

using AoC._2019.Classes;

namespace AoC._2019
{
    [Year(2019)]
    [Day(8)]
    //[Test("123456789012", "1", null)]
    public class Day08 : Puzzle
    {
        private int[] ParseInput(string input) => input.ForEach(s => int.Parse(s.ToString())).ToArray();

        protected override string Part1(string input)
        {
            var parsedInput = ParseInput(input);
            var totalZeros = parsedInput.Where(i => i == 0).Count();

            var layers = new List<List<int>>();
            var position = 0;

            while (position < parsedInput.Length)
            {
                var thisLayer = new List<int>();
                for (var i = 0; i < (6 * 25); ++i) thisLayer.Add(parsedInput[position++]);
                layers.Add(thisLayer);
            }


            var checkLayer = layers.OrderBy(i => i.Where(ii => ii == 0).Count()).First();

            return (checkLayer.Where(i => i == 1).Count() * checkLayer.Where(i => i == 2).Count()).ToString();
        }

        protected override string Part2(string input)
        {
            var parsedInput = ParseInput(input);
            var totalZeros = parsedInput.Where(i => i == 0).Count();

            var layers = new List<List<int>>();
            var position = 0;

            while (position < parsedInput.Length)
            {
                var thisLayer = new List<int>();
                for (var i = 0; i < (6 * 25); ++i) thisLayer.Add(parsedInput[position++]);
                layers.Add(thisLayer);
            }

            var output = new int[25, 6];

            for (var y = 0; y < 6; ++y)
            {
                for (var x = 0; x < 25; ++x)
                {
                    var pos = (y * 25) + x;
                    var layer = 0;

                    while (layers[layer][pos] == 2) layer++;

                    output[x, y] = layers[layer][pos];
                    Console.Write(output[x, y] == 1 ? "#" : " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("What do you see?");

            return Console.ReadLine();

        }
    }
}
