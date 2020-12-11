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
    [Day(10)]
    public class Day10: Puzzle
    {
        private List<int> ParseInput(string input)
        {
            var availableAdaptors = input.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i)).ToList();
            availableAdaptors.Add(0);
            availableAdaptors.Add(availableAdaptors.Max() + 3);

            return availableAdaptors;
        }

        private long GetAdditionalBranchCount(int adaptor, List<int> allAdaptors, Dictionary<int, long> branchCountCache)
        {
            if (!branchCountCache.ContainsKey(adaptor))
            {
                var ongoingAdaptors = allAdaptors.Where(a => a > adaptor && a <= adaptor + 3);
                var ongoingBranches = ongoingAdaptors.Sum(a => GetAdditionalBranchCount(a, allAdaptors, branchCountCache));

                branchCountCache.Add(adaptor, (ongoingAdaptors.Count() < 2 ? 0 : ongoingAdaptors.Count() - 1) + ongoingBranches);
            }
            return branchCountCache[adaptor];
        }

        protected override string Part1(string input)
        {
            var adaptors = ParseInput(input);
            adaptors.Sort();

            var oneJumps = 0;
            var threeJumps = 0;

            for(var i = 0; i < adaptors.Count - 1; ++i)
            {
                if (adaptors[i + 1] - adaptors[i] == 3) threeJumps++;
                else if (adaptors[i + 1] - adaptors[i] == 1) oneJumps++;
            }

            return (oneJumps * threeJumps).ToString();
        }
        protected override string Part2(string input)
        {
            var parsedInput = ParseInput(input);
            var additionalBranchCounts = new Dictionary<int, long>();

            return (GetAdditionalBranchCount(0, parsedInput, additionalBranchCounts) + 1).ToString();
        }
    }
}

