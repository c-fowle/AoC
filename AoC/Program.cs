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

namespace AoC
{
    class Program
    {      
        static void Main(string[] args)
        {
            Console.WriteLine("### ADVENT OF CODE PUZZLE RUNNER ###");
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

            if (puzzleImplementationTypesDictionary.Count == 0)
            {
                Console.WriteLine("No puzzle implementations exist. Press any key to exit...");
                Console.ReadLine();
                return;
            }

            var implementationCount = puzzleImplementationTypesDictionary.Select(kvp => kvp.Value.Count).Sum();
            Console.WriteLine("Found {0} puzzle implementation{1}", implementationCount, implementationCount == 1 ? "" : "s");

            var currentDayEST = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.CreateCustomTimeZone("EST", new TimeSpan(-5, 0, 0), "EST", "EST"));

            Console.WriteLine("Checking for sovled puzzles...");

            // Check for solution file
            if (!Directory.Exists("Data")) Directory.CreateDirectory("Data");
            if (!File.Exists(Path.Combine("Data", "SolvedPuzzles.txt"))) File.Create(Path.Combine("Data", "SolvedPuzzles.txt")).Close();

            var solvedPuzzles = File.ReadAllLines(Path.Combine("Data", "SolvedPuzzles.txt")).ToList();

            Console.WriteLine("Current day in EST: {0}/{1}/{2}", currentDayEST.Year, currentDayEST.Month, currentDayEST.Day);

            var running = true;

            var selectedYear = default(int?);
            var selectedDay = default(int?);
            var partsToRun = new List<int>();
            var autoSubmit = default(bool?);

            if (currentDayEST.Month == 12 && currentDayEST.Day < 26)
            {
                Console.WriteLine("Currently in Advent period! Checking for today's puzzle...");
                if (!puzzleImplementationTypesDictionary.ContainsKey(currentDayEST.Year) || !puzzleImplementationTypesDictionary[currentDayEST.Year].ContainsKey(currentDayEST.Day))
                {
                    Console.WriteLine("No implementation exists year for today's puzzle. Go to https://adventofcode.com/{0}/day/{1} for puzzle details! Proceeding to puzzle selection...", currentDayEST.Year, currentDayEST.Day);
                }
                else
                {
                    if (!solvedPuzzles.Contains(currentDayEST.Year.ToString() + "_" + currentDayEST.Day.ToString() + "_1"))
                    {
                        Console.WriteLine("Part 1 not yet solved - running solution...");
                        Console.WriteLine();
                        selectedYear = currentDayEST.Year;
                        selectedDay = currentDayEST.Day;
                        partsToRun.Add(1);
                        autoSubmit = true;
                    }
                    else if (!solvedPuzzles.Contains(currentDayEST.Year.ToString() + "_" + currentDayEST.Day.ToString() + "_2"))
                    {
                        Console.WriteLine("Part 2 not yet solved - running solution...");
                        Console.WriteLine();
                        selectedYear = currentDayEST.Year;
                        selectedDay = currentDayEST.Day;
                        partsToRun.Add(2);
                        autoSubmit = true;
                    }
                    else Console.WriteLine("Today's puzzle has already been solved! Proceeding to puzzle selection...");
                }
            }
            else Console.WriteLine("Outside of advent period. Proceeding to puzzle selection...");

            while (running)
            {
                while (selectedYear == null)
                {
                    Console.WriteLine();
                    Console.WriteLine("Select year:");
                    puzzleImplementationTypesDictionary.Keys.OrderBy(k => k).ForEach(i => Console.WriteLine("{0}", i));

                    var userInput = Console.ReadLine();
                    var selection = 0;

                    if (!int.TryParse(userInput, out selection) || !puzzleImplementationTypesDictionary.ContainsKey(selection))
                    {
                        Console.WriteLine("Invalid input, please select one of the available options...");
                        Console.WriteLine("Press ESC to quit or any key to continue...");
                        if (Console.ReadKey().Key == ConsoleKey.Escape) return;
                        continue;
                    }

                    selectedYear = selection;
                }

                while (selectedDay == null)
                {
                    Console.WriteLine();
                    Console.WriteLine("Year {0}", selectedYear);
                    Console.WriteLine("Select day:");
                    puzzleImplementationTypesDictionary[selectedYear.Value].Keys.OrderBy(k => k).ForEach(i => Console.WriteLine("{0}", i));

                    var userInput = Console.ReadLine();
                    var selection = 0;

                    if (!int.TryParse(userInput, out selection) || !puzzleImplementationTypesDictionary[selectedYear.Value].ContainsKey(selection))
                    {
                        Console.WriteLine("Invalid input, please select one of the available options...");
                        Console.WriteLine("Press ESC to quit or any key to continue...");
                        if (Console.ReadKey().Key == ConsoleKey.Escape) return;
                        continue;
                    }
                    selectedDay = selection;
                }

                while (partsToRun.Count == 0)
                {
                    Console.WriteLine();
                    Console.Write("Choose puzzle part to run [1/2/(B)oth]: ");
                    var userInput = Console.ReadKey();

                    if (userInput.Key == ConsoleKey.D1 || userInput.Key == ConsoleKey.NumPad1) partsToRun.Add(1);
                    else if (userInput.Key == ConsoleKey.D2 || userInput.Key == ConsoleKey.NumPad2) partsToRun.Add(2);
                    else if (userInput.Key == ConsoleKey.B) partsToRun.AddRange(new[] { 1, 2 });
                    else
                    {
                        Console.WriteLine("Invalid input, please select 1, 2, or (B)oth...");
                        Console.WriteLine("Press ESC to quit or any key to continue...");
                        if (Console.ReadKey().Key == ConsoleKey.Escape) return;
                        continue;
                    }
                }

                while (autoSubmit == null)
                {
                    Console.WriteLine();
                    Console.Write("Automatically submit solution to AoC? [Y/N]: ");
                    var userInput = Console.ReadKey();

                    if (userInput.Key == ConsoleKey.Y) autoSubmit = true;
                    else if (userInput.Key == ConsoleKey.N) autoSubmit = false;
                    else
                    {
                        Console.WriteLine("Invalid input, please select (Y)es or (N)o...");
                        Console.WriteLine("Press ESC to quit or any key to continue...");
                        if (Console.ReadKey().Key == ConsoleKey.Escape) return;
                        continue;
                    }
                }

                Console.Clear();

                var puzzleToRun = Activator.CreateInstance(puzzleImplementationTypesDictionary[selectedYear.Value][selectedDay.Value]) as Puzzle;
                partsToRun.Sort();

                Console.WriteLine("Running solution for {0}/12/{1} part{2} {3} - solution will{4} be submitted on completion", selectedYear, selectedDay, partsToRun.Count == 1 ? "": "s", string.Join(" & ", partsToRun.Select(i => i.ToString())), autoSubmit.Value ? "" : " not");

                foreach (var partToRun in partsToRun.OrderBy(i => i))
                {
                    Console.WriteLine("Part {0}", partToRun);

                    IList<PuzzleResult> puzzleTestResults = null;
                    var exceptionInTests = false;

                    try
                    {
                        puzzleTestResults = puzzleToRun.Test(partToRun);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception during tests for part {0}:", partToRun);
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                        exceptionInTests = true; 
                    }

                    if (!exceptionInTests && (puzzleTestResults == null || puzzleTestResults.Count() == 0 || puzzleTestResults.All(result =>
                    {
                        if (result.SolutionResponse != SolutionResponse.Correct)
                        {
                            Console.WriteLine("Test failed with message: '{0}'", result.FullTextResponse);
                        }
                        return result.SolutionResponse == SolutionResponse.Correct;
                    })))
                    {
                        if (!Directory.Exists(Path.Combine("Data", selectedYear.ToString(), selectedDay.ToString()))) Directory.CreateDirectory(Path.Combine("Data", selectedYear.ToString(), selectedDay.ToString()));
                        if (!File.Exists(Path.Combine("Data", selectedYear.ToString(), selectedDay.ToString(), "SolutionHistory.txt"))) File.Create(Path.Combine("Data", selectedYear.ToString(), selectedDay.ToString(), "SolutionHistory.txt")).Close();

                        var solutionHistory = File.ReadAllLines(Path.Combine("Data", selectedYear.ToString(), selectedDay.ToString(), "SolutionHistory.txt")).Select(s => new SubmittedSolution(s.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))).Where(ss => ss.Part == partToRun).ToList();

                        Console.WriteLine("All tests passed, continuing with solution...");
                        var solutionResult = puzzleToRun.Solve(partToRun, autoSubmit.Value, solutionHistory);

                        Console.WriteLine("Solution:");
                        Console.WriteLine(solutionResult.Solution);
                        Console.WriteLine("Calculated in:");
                        Console.WriteLine("{0}:{1}:{2}.{3}", solutionResult.ExecutionTime.Hours, solutionResult.ExecutionTime.Minutes, solutionResult.ExecutionTime.Seconds, solutionResult.ExecutionTime.Milliseconds);

                        if (solutionResult.RepeatedSubmission || solutionResult.ComparedWithCached)
                        {
                            Console.WriteLine();
                            Console.Write("SUBMISSION SKIPPED - ");
                            Console.WriteLine(solutionResult.RepeatedSubmission ? "Duplicate submission in history" : "Correct solution was already found in submission history");

                        }

                        if (autoSubmit.Value)
                        {
                            Console.WriteLine();
                            Console.Write("Solution was ");

                            switch (solutionResult.SolutionResponse)
                            {
                                case SolutionResponse.Correct:
                                    Console.WriteLine("correct!");
                                    
                                    if (!solvedPuzzles.Contains(selectedYear.Value.ToString() + "_" + selectedDay.Value.ToString() + "_" + partToRun.ToString()))
                                    {
                                        solvedPuzzles.Add(selectedYear.Value.ToString() + "_" + selectedDay.Value.ToString() + "_" + partToRun.ToString());
                                        File.WriteAllLines(Path.Combine("Data", "SolvedPuzzles.txt"), solvedPuzzles);
                                    }

                                    break;
                                case SolutionResponse.IncorrectNoInformation:
                                    Console.WriteLine("incorrect...");
                                    break;
                                case SolutionResponse.IncorrectTooHigh:
                                    Console.WriteLine("incorrect, the answer was too high...");
                                    break;
                                case SolutionResponse.IncorrectTooLow:
                                    Console.WriteLine("incorrect, the answer was too low...");
                                    break;
                                case SolutionResponse.WaitToSubmit:
                                    Console.WriteLine("submitted too soon...");
                                    Console.WriteLine("Wait another {0}m {1}s until submitting again", solutionResult.WaitTime?.Minutes, solutionResult.WaitTime?.Seconds);
                                    break;
                                case SolutionResponse.AlreadySolved:
                                    Console.WriteLine("ignored, the correct solution has already been submitted for this puzzle...");
                                    Console.WriteLine("Attemping to fetch previous solution...");
                                    Console.WriteLine();

                                    var puzzlePageText = Connectivity.FetchPuzzlePage(selectedYear.Value, selectedDay.Value);
                                    var answerParser = new Regex("Your puzzle answer was <code>(?<answer>.*?)<\\/code>", RegexOptions.Singleline);

                                    var answerMatches = answerParser.Matches(puzzlePageText);

                                    if (answerMatches.Count < partToRun) Console.WriteLine("The submitted solution for part {0} was ignored, but {1} answer{2} were found on the page...", partToRun, answerMatches.Count, answerMatches.Count == 1 ? "" : "s"); 

                                    for (var matchCounter = 0; matchCounter < answerMatches.Count; ++matchCounter)
                                    {
                                        var thisMatch = answerMatches[matchCounter];

                                        solutionHistory.Add(new SubmittedSolution(matchCounter + 1, thisMatch.Groups["answer"].Value, SolutionResponse.Correct));
                                        if (partToRun == (matchCounter + 1))
                                        {
                                            Console.Write("By comparison with previous submission, this solution was ");
                                            if (solutionResult.Solution == thisMatch.Groups["answer"].Value) Console.WriteLine("correct!");
                                            else
                                            {
                                                var parsedSubmission = 0;
                                                var parsedHistoric = 0;

                                                if (int.TryParse(solutionResult.Solution, out parsedSubmission) && int.TryParse(thisMatch.Groups["answer"].Value, out parsedHistoric))
                                                {
                                                    if (parsedSubmission > parsedHistoric) Console.WriteLine("incorrect, the answer was too high...");
                                                    else Console.WriteLine("incorrect, the answer was too low...");
                                                }
                                                else Console.WriteLine("incorrect...");
                                            }
                                        }
                                    }

                                    if (answerMatches.Count > 0) File.WriteAllLines(Path.Combine("Data", selectedYear.ToString(), selectedDay.ToString(), "SolutionHistory.txt"), solutionHistory.Select(ss => ss.ToString()).ToList());

                                    break;
                                case SolutionResponse.NotSubmitted:
                                    Console.WriteLine("was not submitted...");
                                    break;
                                case SolutionResponse.Unrecognised:
                                default:
                                    Console.WriteLine("unknown - unable to parse response");
                                    Console.WriteLine("Full response from AoC:");
                                    Console.WriteLine(solutionResult.FullTextResponse);
                                    break;
                            }

                            var validResponses = new List<SolutionResponse> { SolutionResponse.Correct, SolutionResponse.IncorrectNoInformation, SolutionResponse.IncorrectTooHigh, SolutionResponse.IncorrectTooLow };

                            if (validResponses.Any(valid => valid == solutionResult.SolutionResponse) && solutionHistory.All(ss => ss.Response != SolutionResponse.Correct) && solutionHistory.All(ss => ss.Solution != solutionResult.Solution))
                            {
                                solutionHistory.Add(new SubmittedSolution(partToRun, solutionResult.Solution, solutionResult.SolutionResponse));
                                File.WriteAllLines(Path.Combine("Data", selectedYear.ToString(), selectedDay.ToString(), "SolutionHistory.txt"), solutionHistory.Select(ss => ss.ToString()).ToList());
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("One or more tests failed, revise solution...");
                    }

                    Console.WriteLine();
                }

                Console.WriteLine("Execution complete!");
                Console.WriteLine("Press ESC to exit or any key to return to puzzle selection...");
                if (Console.ReadKey().Key == ConsoleKey.Escape) return;
                else
                {
                    selectedYear = default(int?);
                    selectedDay = default(int?);
                    partsToRun = new List<int>();
                    autoSubmit = default(bool?);
                }
            }
        }
    }
}
