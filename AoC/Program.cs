using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reflection;

using AoC.Common;
using AoC.Common.Attributes;
using AoC.Common.ExtensionMethods;

using AoC._2017.Day05;

namespace AoC
{
    //enum Direction
    //{
    //    Up = 0,
    //    Down = 1,
    //    Left = 2,
    //    Right = 3
    //}

    //class State
    //{
    //    public int X { get; set; }
    //    public int Y { get; set; }
    //    public string Path { get; set; }
        
    //    public State(int x, int y, string path)
    //    {
    //        X = x;
    //        Y = y;
    //        Path = path;
    //    }

    //    public IList<State> GetChildren (string passcode, char[] openChars, int gridSize=4)
    //    {
    //        var result = new List<State>();
    //        var doorStates = new bool[4];

    //        using (var md5Hash = MD5.Create())
    //        {
    //            byte[] data = md5Hash.ComputeHash(Encoding.ASCII.GetBytes(passcode + this.Path));
    //            if (data.Length < 2) return result;

    //            var dataStr = data[0].ToString("X2") + data[1].ToString("X2");
    //            if (dataStr.Length < 4) return result;

    //            for (var i = 0; i < 4; ++i) doorStates[i] = openChars.Contains(dataStr[i]);
    //        }

    //        if (this.Y > 0 && doorStates[(int)Direction.Up]) result.Add(new State(this.X, this.Y - 1, this.Path + "U"));
    //        if (this.Y < (gridSize - 1) && doorStates[(int)Direction.Down]) result.Add(new State(this.X, this.Y + 1, this.Path + "D"));
    //        if (this.X > 0 && doorStates[(int)Direction.Left]) result.Add(new State(this.X - 1, this.Y, this.Path + "L"));
    //        if (this.X < (gridSize - 1) && doorStates[(int)Direction.Right]) result.Add(new State(this.X + 1, this.Y, this.Path + "R"));

    //        return result;
    //    }
    //}

    class SomeAttribute : Attribute
    {
        public SomeAttribute() { }
    }

    class Program
    {
        //static async Task<string> GetData()
        //{
        //    var cookieContainer = new CookieContainer();
        //    var baseAddress = new Uri("https://adventofcode.com/");
        //    cookieContainer.Add(baseAddress, new Cookie("session", "53616c7465645f5f7ea0accac870d924561d4d07ee2e53daac23fde9844e78753231821078caa884d5a8b4a591b45b6c"));

        //    var client = new HttpClient(
        //        new HttpClientHandler
        //        {
        //            CookieContainer = cookieContainer,
        //            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        //        });
        //    client.BaseAddress = baseAddress;

        //    var data = "";

        //    //var request = new HttpRequestMessage(HttpMethod.Get, );
        //    var response = await client.GetAsync("https://adventofcode.com/2016/day/17/input");

        //    return await response.Content.ReadAsStringAsync();
        //}

        

        static void Main(string[] args)
        {
            Console.WriteLine("Setting up...");

            Console.WriteLine("Building puzzle list...");

            var puzzleType = typeof(Puzzle);
            var puzzleImplementationTypesDictionary = new Dictionary<int, Dictionary<int, Type>>();
            Assembly.GetExecutingAssembly().GetTypes().Where(t => (!(t.IsAbstract || t.IsInterface)) && puzzleType.IsAssignableFrom(t)).ForEach(t =>
            {
                var yearAttribute = t.GetCustomAttribute<YearAttribute>();
                if (yearAttribute == null)
                {
                    Console.WriteLine("Puzzle implementation {0}.{1} did not have a year attribute", t.Namespace, t.Name);
                    return;
                }

                var dayAttribute = t.GetCustomAttribute<DayAttribute>();
                if (dayAttribute == null)
                {
                    Console.WriteLine("Puzzle implementation {0}.{1} did not have a day attribute", t.Namespace, t.Name);
                    return;
                }

                if (!puzzleImplementationTypesDictionary.ContainsKey(yearAttribute.YearValue)) puzzleImplementationTypesDictionary.Add(yearAttribute.YearValue, new Dictionary<int, Type>());

                if (puzzleImplementationTypesDictionary[yearAttribute.YearValue].ContainsKey(dayAttribute.DayValue))
                {
                    Console.WriteLine("Multiple puzzle implementations exist for the same day: Year {0}, Day {1}", yearAttribute.YearValue, dayAttribute.DayValue);
                    return;
                }

                puzzleImplementationTypesDictionary[yearAttribute.YearValue].Add(dayAttribute.DayValue, t);
            });

            var currentDayEST = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.CreateCustomTimeZone("EST", new TimeSpan(-5, 0, 0), "EST", "EST"));

            Console.WriteLine("Checking for sovled puzzles...");

            // Check for solution file
            if (!Directory.Exists("Data")) Directory.CreateDirectory("Data");
            if (!File.Exists(Path.Combine("Data", "SolvedPuzzles.txt"))) File.Create(Path.Combine("Data", "SolvedPuzzles.txt"));

            var solvedPuzzles = File.ReadAllLines(Path.Combine("Data", "SolvedPuzzles.txt"));

            Console.WriteLine("Current day in EST: {0}/{1}/{2}", currentDayEST.Year, currentDayEST.Month, currentDayEST.Day);

            Type implementationToRun = null;
            int partToRun = 0;

            if (currentDayEST.Month == 12 && currentDayEST.Day < 26)
            {
                Console.WriteLine("Currently in Advent period! Checking for today's puzzle...");
                if (!puzzleImplementationTypesDictionary.ContainsKey(currentDayEST.Year) || !puzzleImplementationTypesDictionary[currentDayEST.Year].ContainsKey(currentDayEST.Day))
                {
                    Console.WriteLine("No implementation exists year for today's puzzle. Go to https://adventofcode.com/{0}/day/{1} for puzzle details! Proceeding to puzzle selection...", currentDayEST.Year, currentDayEST.Day);
                }
                else
                {
                    var thisDaySolutionRecord = solvedPuzzles.Where(s => s.StartsWith(currentDayEST.Year.ToString() + "_" + currentDayEST.Day.ToString() + "_"));
                    if (thisDaySolutionRecord.Count() == 0)
                    {
                        Console.WriteLine("No solution recorded for today");
                    }
                }
            }
            
            var solvedList = File.ReadAllText(Path.Combine("Data", "SolvedPuzzles.txt"));


            Console.WriteLine("Searching for puzzle implementations...");



            Console.WriteLine("{0} puzzle implementation{1} found", puzzleImplementationTypesDictionary.ForEach(kvp => kvp.Value.Values.Count).Sum(), puzzleImplementationTypesDictionary.ForEach(kvp => kvp.Value.Values.Count).Sum() == 1 ? "" : "s");
                
        }
    }
}
