using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AoC.Common;
using AoC.Common.Attributes;
using AoC.Common.ExtensionMethods;

namespace AoC._2020
{
    [Year(2020)]
    [Day(7)]
    public class Day07: Puzzle
    {
        private Dictionary<string, Dictionary<string, int>> ParseInput(string input)
        {
            input = Regex.Replace(input, @"(\.| bags*)", "");
            input = Regex.Replace(input, @" contain ", ", ");
            var inputLines = input.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var splitInputLines = inputLines.Select(l => l.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries)).ToList();

            var ruleParser = new Regex(@"(\d*) (.*)");

            return splitInputLines.ToDictionary(rule => rule[0], rule =>
            {
                var ruleParts = new Dictionary<string, int>();
                if (rule[1] == "no other") return ruleParts;

                for (var counter = 1; counter < rule.Length; ++counter)
                {
                    var ruleMatch = ruleParser.Match(rule[counter]);
                    ruleParts.Add(ruleMatch.Groups[2].Value, int.Parse(ruleMatch.Groups[1].Value));
                }

                return ruleParts;
            });
        }

        private List<string> GetParents(string colour, Dictionary<string, Dictionary<string, int>> rules, Dictionary<string, List<string>> parentLookup)
        {
            if (!parentLookup.ContainsKey(colour))
            {
                parentLookup.Add(colour, new List<string>());

                var parents = rules.Where(kvp => kvp.Value.ContainsKey(colour)).Select(kvp => kvp.Key);
                parents.ForEach(p =>
                {
                    if (!parentLookup[colour].Contains(p)) parentLookup[colour].Add(p);
                    GetParents(p, rules, parentLookup).ForEach(pp =>
                    {
                        if (!parentLookup[colour].Contains(pp)) parentLookup[colour].Add(pp);
                    });
                });
            }
            return parentLookup[colour];
        }

        private int GetChildCount(string colour, Dictionary<string, Dictionary<string, int>> rules, Dictionary<string, int> childCountLookup)
        {
            if (!childCountLookup.ContainsKey(colour))
            {
                childCountLookup[colour] = 0;
                rules[colour].ForEach(kvp =>
                {
                    var childChildCount = GetChildCount(kvp.Key, rules, childCountLookup);
                    childCountLookup[colour] += (childChildCount + 1) * kvp.Value;
                });
            }
            return childCountLookup[colour];
        }

        protected override string Part1(string input)
        {
            var parsedInput = ParseInput(input);
            var parentLookup = new Dictionary<string, List<string>>();
            return GetParents("shiny gold", parsedInput, parentLookup).Count().ToString();
        }
        protected override string Part2(string input)
        {
            var parsedInput = ParseInput(input);
            var childCountLookup = new Dictionary<string, int>();
            return GetChildCount("shiny gold", parsedInput, childCountLookup).ToString();
        }
    }
}

