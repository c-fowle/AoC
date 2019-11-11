using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AoC.Common.Exceptions;
using AoC.Common.ExtensionMethods;

namespace AoC.Common
{
    public static class Connectivity
    {
        private static HttpClient _Client { get; set; }
        public static HttpClient Client {
            get
            {
                if (_Client is null)
                {
                    if (!File.Exists(Path.Combine("Data", "AoCKey.txt"))) throw new NoAoCKeyException();
                    var aocSessionKey = File.ReadAllText(Path.Combine("Data", "AocKey.txt")).Trim();

                    var cookieContainer = new CookieContainer();
                    var baseAddress = new Uri("https://adventofcode.com");
                    cookieContainer.Add(baseAddress, new Cookie("session", aocSessionKey));

                    _Client = new HttpClient(
                        new HttpClientHandler
                        {
                            CookieContainer = cookieContainer,
                            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                        });
                    _Client.BaseAddress = baseAddress;
                }
                return _Client;
            }
        }

        private static async Task<string> DownloadInput(int year, int day)
        {
            var downloadAttempts = 0;

            while (downloadAttempts < 10)
            {
                var response = await Client.GetAsync(String.Format("https://adventofcode.com/{0}/day/{1}/input", year, day));
                if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();
                else
                {
                    Console.WriteLine("Request to get input data responded with status code: " + response.StatusCode.ToString());
                    ++downloadAttempts;
                }
            }

            throw new InputGetRequestRetriesExceededException();
        }

        public static string FetchInput(int year, int day)
        {
            // Create the necessary year directory if it does not exist
            if (!Directory.Exists(Path.Combine("Data", year.ToString()))) Directory.CreateDirectory(Path.Combine("Data", year.ToString()));
            
            // Check if data has already been downloaded and stored -> if not, fetch data and return it
            if (!File.Exists(Path.Combine("Data", year.ToString(), day.ToString() + ".txt")))
            {
                var input = DownloadInput(year, day).GetAwaiter().GetResult()?.Trim();

                using (var inputFile = File.Create(Path.Combine("Data", year.ToString(), day.ToString() + ".txt")))
                using (var writer = new StreamWriter(inputFile))
                {
                    writer.Write(input);
                }

                return input;
            }

            // If file already exists, read data from file to return
            return File.ReadAllText(Path.Combine("Data", year.ToString(), day.ToString() + ".txt")).Trim();
        }

        private static async Task<HttpResponseMessage> PostSolutionRequest (int year, int day, string solution)
        {
            var postContent = new FormUrlEncodedContent(
                new Dictionary<string, string> {
                    { "answer", solution }
                }.ToList());
            return await Client.PostAsync(String.Format("https://adventofcode.com/{0}/day/{1}/answer", year, day), postContent);
        }

        public static bool SubmitSolution(int year, int day, string solution)
        {
            var solutionResponse = PostSolutionRequest(year, day, solution).GetAwaiter().GetResult();

            Console.WriteLine(solutionResponse.StatusCode);
            Console.WriteLine(solutionResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            Console.WriteLine();

            return false;
        }
    }
}
