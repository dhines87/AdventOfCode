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
        }

        internal static int CalibrateFrequency(List<int> frequencyChanges)
        {
            return frequencyChanges.Aggregate(0, (current, frequency) => current + frequency);
        }
    }
}
