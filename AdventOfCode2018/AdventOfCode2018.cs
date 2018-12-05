using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode2018
{
    internal class AdventOfCode2018
    {
        private static void Main(string[] args)
        {
            var days = new List<IAdventCode>()
            {
                new Day1(),
                new Day2(),
                new Day3(),
                new Day4()
            };

            days.ForEach(RunDay);
        }

        internal interface IAdventCode
        {
            void Read();
            void SolvePart1();
            void SolvePart2();
        }

        private static void RunDay(IAdventCode day)
        {
            day.Read();
            day.SolvePart1();
            day.SolvePart2();
        }

        internal sealed class Day1 : IAdventCode
        {
            private static readonly List<int> FrequencyChanges = new List<int>();

            public void Read()
            {
                var file = new StreamReader(@"c:\TempPath\AdventOfCode\Day1Input.txt");
                string line;

                while ((line = file.ReadLine()) != null)
                {
                    FrequencyChanges.Add(int.Parse(line, NumberStyles.AllowLeadingSign));
                }
            }

            public void SolvePart1() => Console.WriteLine("Day 1 Part 1: {0}", CalibrateFrequency());

            public void SolvePart2() => Console.WriteLine("Day 1 Part 2: {0}", CalibrateDuplicateFrequency());

            private static int CalibrateFrequency() => FrequencyChanges.Aggregate(0, (current, frequency) => current + frequency);

            private static int CalibrateDuplicateFrequency()
            {
                var frequencyHash = new HashSet<int>();
                var currentFrequency = 0;
                var duplicateFound = false;

                do
                {
                    foreach (var frequency in FrequencyChanges)
                    {
                        currentFrequency += frequency;

                        if (!frequencyHash.Add(currentFrequency))
                        {
                            duplicateFound = true;
                            break;
                        }
                    }
                } while (!duplicateFound);

                return currentFrequency;
            }
        }

        internal sealed class Day2 : IAdventCode
        {
            private static readonly List<string> Ids = new List<string>();

            public void Read()
            {
                var file = new StreamReader(@"c:\TempPath\AdventOfCode\Day2Input.txt");
                string line;

                while ((line = file.ReadLine()) != null)
                {
                    Ids.Add(line);
                }
            }

            public void SolvePart1() => Console.WriteLine("Day 2 Part 1:  {0}", GenerateCheckSum());

            public void SolvePart2() => Console.WriteLine("Day 2 Part 2: {0}", GetCommonChars());

            private static int GenerateCheckSum()
            {
                int twos = 0,
                    threes = 0;

                foreach (var id in Ids)
                {
                    var result = id
                        .Where(char.IsLetter)
                        .GroupBy(c => c)
                        .ToArray();

                    if (result.Any(g => g.Count() == 2))
                    {
                        ++twos;
                    }

                    if (result.Any(g => g.Count() == 3))
                    {
                        ++threes;
                    }
                }

                return twos * threes;
            }

            private static string GetCommonChars()
            {
                var commonIds = Ids
                    .SelectMany((leftWord, outerIndex) =>
                        Ids
                            .Where((rightWord, innerIndex) => outerIndex > innerIndex)
                            .Select(rightWord => new { LeftWord = leftWord, RightWord = rightWord }))
                    .GroupBy(pair => HammingDistance(pair.LeftWord, pair.RightWord))
                    .Where(g => g.Key == 1)
                    .Select(g => g.ToList())
                    .First()
                    .First();

                var removeAtIndex = commonIds.LeftWord.Zip(commonIds.RightWord, (leftChar, rightChar) => leftChar == rightChar)
                    .TakeWhile(b => b).Count();

                return commonIds.LeftWord.Remove(removeAtIndex, 1);
            }

            private static int HammingDistance(string left, string right) =>
                left.Zip(right, (leftChar, rightChar) => leftChar - rightChar == 0 ? 0 : 1).Sum();
        }

        internal sealed class Day3 : IAdventCode
        {
            private const int NumberOfIntersections = 2;

            private sealed class Claim
            {
                public readonly int Id;
                public readonly int Left;
                public readonly int Top;
                public readonly int Width;
                public readonly int Height;

                public Claim(int id, int left, int top, int width, int height)
                {
                    Id = id;
                    Left = left;
                    Top = top;
                    Width = width;
                    Height = height;
                }
            }

            private static readonly List<Claim> Claims = new List<Claim>();

            public void Read()
            {
                var file = new StreamReader(@"c:\TempPath\AdventOfCode\Day3Input.txt");
                string line;

                while ((line = file.ReadLine()) != null)
                {
                    var splits = line.Split(' ', '#', '@', ',', ':', 'x').Where(s => s.Length > 0).ToArray();

                    Claims.Add(new Claim(int.Parse(splits[0]),
                        int.Parse(splits[1]),
                        int.Parse(splits[2]),
                        int.Parse(splits[3]),
                        int.Parse(splits[4])));
                }
            }

            public void SolvePart1() => Console.WriteLine("Day 3 Part 1: {0}", GetAreaOfIntersectingClaims());

            public void SolvePart2() => Console.WriteLine("Day 3 Part 2: {0}", GetNonIntersectingClaimId());

            private static int GetAreaOfIntersectingClaims()
            {
                return GetIntersectingCoordinates()
                    .Count();
            }

            private static int GetNonIntersectingClaimId()
            {
                var intersectingCoordinates = GetIntersectingCoordinates()
                    .Select(c => c.Key)
                    .ToHashSet();

                return Claims
                    .First(c => !Enumerable.Range(c.Left, c.Width)
                        .SelectMany(x => Enumerable.Range(c.Top, c.Height)
                            .Select(y => (x, y)))
                        .Any(coordinate => intersectingCoordinates.Contains(coordinate)))
                    .Id;
            }

            private static IEnumerable<IGrouping<(int, int), (int, int)>> GetIntersectingCoordinates()
            {
                return Claims
                    .SelectMany(c => Enumerable.Range(c.Left, c.Width)
                        .SelectMany(x => Enumerable.Range(c.Top, c.Height)
                            .Select(y => (x, y))))
                    .GroupBy(c => c)
                    .Where(c => c.Count() >= NumberOfIntersections);
            }
        }

        internal sealed class Day4 : IAdventCode
        {
            private sealed class GuardActivity
            {
                public readonly DateTime TimeStamp;
                public readonly string[] Activity;

                public GuardActivity(DateTime timeStamp, string[] activity)
                {
                    TimeStamp = timeStamp;
                    Activity = activity;
                }
            }

            private sealed class GuardDayNap
            {
                public readonly int Id;
                public readonly DateTime Day;
                public readonly int MinutesAsleep;
            }

            public void Read()
            {
                var file = new StreamReader(@"c:\TempPath\AdventOfCode\Day4Input.txt");
                string line;
                var guardActivities = new List<GuardActivity>();

                while ((line = file.ReadLine()) != null)
                {
                    var dateTimeSplit = line
                        .Substring(1, line.IndexOf(']') - 1)
                        .Split('-', ' ', ':');

                    var dateTime = new DateTime(int.Parse(dateTimeSplit[0]), 
                        int.Parse(dateTimeSplit[1]), 
                        int.Parse(dateTimeSplit[2]), 
                        int.Parse(dateTimeSplit[3]),
                        int.Parse(dateTimeSplit[4]), 
                        0);

                    var activity = line
                        .Substring(line.IndexOf(']') + 2)
                        .Split(' ');

                    guardActivities.Add(new GuardActivity(dateTime, activity));
                }

                guardActivities = guardActivities
                    .OrderBy(ga => ga.TimeStamp)
                    .ToList();
            }

            public void SolvePart1()
            {
            }

            public void SolvePart2()
            {
                throw new NotImplementedException();
            }
        }
    }
}
