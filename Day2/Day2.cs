using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Day2
{
    internal class Day2
    {
        static void Main(string[] args)
        {
            var file = new StreamReader(@"c:\TempPath\AdventOfCode\Day2Input.txt");
            string line;
            var ids = new List<string>();

            while ((line = file.ReadLine()) != null)
            {
                ids.Add(line);
            }

            Console.WriteLine(GenerateCheckSum(ids));
            Console.WriteLine(GetCommonChars(ids));
        }

        internal static int GenerateCheckSum(List<string> ids)
        {
            int twos = 0, 
                threes = 0;

            foreach (var id in ids)
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

        internal static string GetCommonChars(List<string> ids)
        {
            var commonIds = ids
                .SelectMany((leftWord, outerIndex) =>
                    ids
                        .Where((rightWord, innerIndex) => outerIndex > innerIndex)
                        .Select(rightWord => new {LeftWord = leftWord, RightWord = rightWord}))
                .GroupBy(pair => HammingDistance(pair.LeftWord, pair.RightWord))
                .Where(g => g.Key == 1)
                .Select(g => g.ToList())
                .First()
                .First();

            var removeAtIndex = commonIds.LeftWord.Zip(commonIds.RightWord, (leftChar, rightChar) => leftChar == rightChar)
                       .TakeWhile(b => b).Count();

            return commonIds.LeftWord.Remove(removeAtIndex, 1);
        }

        internal static int HammingDistance(string left, string right) =>
            left.Zip(right, (leftChar, rightChar) => leftChar - rightChar == 0 ? 0 : 1).Sum();
    }
}
