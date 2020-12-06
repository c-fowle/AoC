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
    [Day(6)]
    public class Day06: Puzzle
    {
        private List<char[][]> ParseInput(string input) => input.Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(ss => ss.ToArray()).ToArray()).ToList();

        protected override string Part1(string input)
        {
            var parsedInput = ParseInput(input);
            return parsedInput.Select(answersByPerson =>
            {
                var uniqueAnswers = new List<char>();

                answersByPerson.ForEach(answers => answers.ForEach(a =>
                {
                    if (!uniqueAnswers.Contains(a)) uniqueAnswers.Add(a);
                }));

                return uniqueAnswers.Count;
            }).Sum().ToString();
        }
        protected override string Part2(string input)
        {
            var parsedInput = ParseInput(input);
            return parsedInput.Select(answersByPerson =>
            {
                if (answersByPerson.GetLength(0) == 1) return answersByPerson[0].Length;

                var intersect = answersByPerson[0].Intersect(answersByPerson[1]);
                for (var count = 2; count < answersByPerson.GetLength(0); ++count) intersect = intersect.Intersect(answersByPerson[count]);
                return intersect.Count();
            }).Sum().ToString();
        }
    }
}

