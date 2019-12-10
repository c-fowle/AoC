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
    [Year(2019)]
    [Day(4)]
    public class Day04 : _2019Puzzle
    {
        private int[] ParseInput(string input) => input.Split('-').Select(s => int.Parse(s)).ToArray();
        private int[] GetDigits(int number)
        {
            var digitCount = 1;

            while (number > Math.Pow(10, digitCount)) ++digitCount;

            var digits = new int[digitCount];

            for (var d = (digitCount - 1); d > -1; --d)
            {
                var thisDigit = (int)Math.Floor((double)number / (Math.Pow(10, d)));
                digits[digitCount - (d + 1)] = thisDigit;
                number = (int)(number - (thisDigit * (Math.Pow(10, d))));
            }

            return digits;
        }
        private int CombineDigits(int[] digits)
        {
            var exp = digits.Length - 1;
            var number = 0;

            for(var d = 0; d < digits.Length; ++d) number += digits[d] * (int)Math.Pow(10, exp--);

            return number;
        }

        protected override string Part1(string input)
        {
            var parsedInput = ParseInput(input);
            var validPasses = new List<int>();

            for (var pass = parsedInput[0]; pass < parsedInput[1]; ++pass)
            {
                var digits = GetDigits(pass);

                var hasDouble = false;
                var invalid = false;

                for (var d = 0; d < digits.Length - 1; ++d)
                {
                    if (digits[d] > digits[d + 1])
                    {
                        invalid = true;
                        for (var dd = d + 1; dd < digits.Length; ++dd) digits[dd] = digits[d];
                        pass = CombineDigits(digits) - 1;
                        break;
                    }
                    if (digits[d] == digits[d + 1])
                    {
                        hasDouble = true;
                        d += 1;
                    }
                }

                if (hasDouble && !invalid) validPasses.Add(pass);
            }

            return validPasses.Count.ToString();
        }

        protected override string Part2(string input)
        {
            var parsedInput = ParseInput(input);
            var validPasses = new List<int>();

            for (var pass = parsedInput[0]; pass < parsedInput[1]; ++pass)
            {
                var digits = GetDigits(pass);

                var hasDouble = false;
                var invalid = false;

                for (var d = 0; d < digits.Length - 1; ++d)
                {
                    if (digits[d] > digits[d + 1])
                    {
                        invalid = true;
                        for (var dd = d + 1; dd < digits.Length; ++dd) digits[dd] = digits[d];
                        pass = CombineDigits(digits) - 1;
                        break;
                    }

                    var identicalDigits = 1;

                    while (d < (digits.Length - 1) && digits[d] == digits[d + 1])
                    {
                        identicalDigits += 1;
                        d += 1;
                    }

                    hasDouble |= identicalDigits == 2;
                }


                if (hasDouble && !invalid) validPasses.Add(pass);
            }

            return validPasses.Count.ToString();
        }
    }
}
