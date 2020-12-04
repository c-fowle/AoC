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
    [Day(4)]
    [Test(
        "ecl:gry pid:860033327 eyr:2020 hcl:#fffffd byr:1937 iyr:2017 cid:147 hgt:183cm\n\n" + 
        "iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884\nhcl:#cfa07d byr:1929\n\n" +
        "hcl:#ae17e1 iyr:2013\neyr:2024\necl:brn pid:760753108 byr:1931\nhgt:179cm\n\n" +
        "hcl:#cfa07d eyr:2025 pid:166559648\niyr:2011 ecl:brn hgt:59in", "2", "2")]
    [Test(
        "pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980\nhcl:#623a2f\n\n" +
        "eyr:2029 ecl:blu cid:129 byr:1989\niyr:2014 pid:896056539 hcl:#a97842 hgt:165cm\n\n" +
        "hcl:#888785\nhgt:164cm byr:2001 iyr:2015 cid:88\npid:545766238 ecl:hzl\neyr:2022\n\n" +
        "iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719", "4", "4")]
    [Test(
        "eyr:1972 cid:100\nhcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926\n\n" +
        "iyr:2019\nhcl:#602927 eyr:1967 hgt:170cm\necl:grn pid:012533040 byr:1946\n\n" +
        "hcl:dab227 iyr:2012\necl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277\n\n" +
        "hgt:59cm ecl:zzz\neyr:2038 hcl:74454a iyr:2023\npid:3556412378 byr:2007", "4", "0")]
    public class Day04 : Puzzle
    {
        protected override string Part1(string input) => new Regex(@"(?:byr:\S*?\s{0,1}()|iyr:\S*?\s{0,1}()|eyr:\S*?\s{0,1}()|hgt:\S*?\s{0,1}()|hcl:\S*?\s{0,1}()|ecl:\S*?\s{0,1}()|pid:\S*?\s{0,1}()|(?:cid:\S*?\s{0,1}|)()){8}\1\2\3\4\5\6\7\8\n{0,2}").Matches(input).Count.ToString();
        protected override string Part2(string input)
        {
            var validationPatterns = new Dictionary<string, string>()
            {
                { "byr", @"(?:19[2-9]\d|200[0-2])" },
                { "iyr", @"20(?:1\d|20)" },
                { "eyr", @"20(?:2\d|30)" },
                { "hgt", @"(?:1(?:[5-8]\d|9[0-3])cm|(?:59|6\d|7[0-6])in)" },
                { "hcl", @"#[a-f0-9]{6}" },
                { "ecl", @"(?:amb|blu|brn|gry|grn|hzl|oth)" },
                { "pid", @"\d{9}"}
            };
            var fullPattern = "(?:" + string.Join("|", validationPatterns.Select(kvp => kvp.Key + ":" + kvp.Value + @"\s{0,1}()")) + @"|(?:cid:\S*?\s{0,1}|)()){8}\1\2\3\4\5\6\7\8\n{0,2}";
            var regex = new Regex(fullPattern);
            return regex.Matches(input).Count.ToString();
        }
    }
}
