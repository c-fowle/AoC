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

namespace AoC._2020
{
    [Year(2020)]
    [Day(2)]
    [Test("1-3 a: abcde\n1-3 b: cdefg\n2-9 c: ccccccccc", "2", "1")]
    public class Day02 : Puzzle    
    {
        private List<string[]> ParseInput(string input) => input.Replace(':', ' ').Split('\n').Select(s => s.Split(new[] { '-', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray()).ToList();

        protected override string Part1(string input)
        {
            var parsedInput = ParseInput(input);
            var validPasswords = 0;     

            parsedInput.ForEach(passwordDetails =>
            {
                var charCount = passwordDetails[3].Count(c => c == passwordDetails[2][0]);
                if (charCount >= int.Parse(passwordDetails[0]) && charCount <= int.Parse(passwordDetails[1]))
                {
                    validPasswords++;
                }
            });

            return validPasswords.ToString();
        }

        protected override string Part2(string input)
        {
            var parsedInput = ParseInput(input);
            var validPasswords = 0;

            parsedInput.ForEach(passwordDetails =>
            {
                if (passwordDetails[3][int.Parse(passwordDetails[0]) - 1] == (char)passwordDetails[2][0] ^ passwordDetails[3][int.Parse(passwordDetails[1]) - 1] == (char)passwordDetails[2][0])
                {
                    validPasswords++;
                }
            });

            return validPasswords.ToString();
        }
    }
}
 