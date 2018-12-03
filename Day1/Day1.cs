using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Day1
{
    internal class Day1
    {
        public static void Main(string[] args)
        {
            var file = new StreamReader(@"c:\TempPath\AdventOfCode\Day1Input.txt");
            string line;
            var frequencyChanges = new List<int>();

            while ((line = file.ReadLine()) != null)
            {
                frequencyChanges.Add(int.Parse(line, NumberStyles.AllowLeadingSign));
            }

            Console.WriteLine("Frequency result: {0}", CalibrateFrequency(frequencyChanges));
            Console.WriteLine("First duplicate frequency: {0}", CalibrateDuplicateFrequency(frequencyChanges));
        }

        internal static int CalibrateFrequency(List<int> frequencyChanges)
        {
            return frequencyChanges.Aggregate(0, (current, frequency) => current + frequency);
        }

        internal static int CalibrateDuplicateFrequency(List<int> frequencyChanges)
        {
            var frequencyHash = new HashSet<int>();
            var currentFrequency = 0;
            var duplicateFound = false;

            do
            {
                foreach (var frequency in frequencyChanges)
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
}
