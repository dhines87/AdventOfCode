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
            Console.WriteLine(GenerateCheckSum(new List<string>() { "abcdef", "bababc", "abbcde", "abcccd", "aabcdd", "abcdee", "ababab" }));
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
    }
}
