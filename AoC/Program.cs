using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using System.Security.Cryptography;

using HtmlAgilityPack;

using AoC.Common;
using AoC.Common.ExtensionMethods;

namespace AoC
{
    enum Direction
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3
    }

    class State
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Path { get; set; }
        
        public State(int x, int y, string path)
        {
            X = x;
            Y = y;
            Path = path;
        }

        public IList<State> GetChildren (string passcode, char[] openChars, int gridSize=4)
        {
            var result = new List<State>();
            var doorStates = new bool[4];

            using (var md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.ASCII.GetBytes(passcode + this.Path));
                if (data.Length < 2) return result;

                var dataStr = data[0].ToString("X2") + data[1].ToString("X2");
                if (dataStr.Length < 4) return result;

                for (var i = 0; i < 4; ++i) doorStates[i] = openChars.Contains(dataStr[i]);
            }

            if (this.Y > 0 && doorStates[(int)Direction.Up]) result.Add(new State(this.X, this.Y - 1, this.Path + "U"));
            if (this.Y < (gridSize - 1) && doorStates[(int)Direction.Down]) result.Add(new State(this.X, this.Y + 1, this.Path + "D"));
            if (this.X > 0 && doorStates[(int)Direction.Left]) result.Add(new State(this.X - 1, this.Y, this.Path + "L"));
            if (this.X < (gridSize - 1) && doorStates[(int)Direction.Right]) result.Add(new State(this.X + 1, this.Y, this.Path + "R"));

            return result;
        }
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
            var response_doc = new HtmlDocument();
            response_doc.Load("C:\\aoc_response.htm");
            var response_text = response_doc.DocumentNode.ChildNodes.Single(n => n.Name == "html").ChildNodes.Single(n => n.Name == "body").ChildNodes.Single(n => n.Name == "main").InnerText.Trim();

            Console.WriteLine(response_text.StartsWith("That's"));

            Console.WriteLine(response_text);
            Console.ReadLine();

            var input = "";
            var paths = new List<string>();
            var openChars = (new char[] { 'B', 'C', 'D', 'E', 'F' });

            var availableStates = new List<State> { new State(0, 0, input) };
            var exitFound = false;

            while (!exitFound)
            {
                var newStates = new List<State>();

                if (availableStates.Count == 0)
                {
                    Console.WriteLine("No solution exists...");
                    break;
                }

                foreach (var s in availableStates)
                {
                    newStates.AddRange(s.GetChildren("", openChars));
                }

                var complete = newStates.Where(s => s.X == 3 && s.Y == 3);

                if (complete.Any())
                {
                    foreach (var s in complete) Console.WriteLine(s.Path);
                    exitFound = true;
                }
                else
                {
                    availableStates = newStates;
                }
                    
            }

            Console.Read();
        }
    }
}
