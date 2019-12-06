using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace aoc2019
{
    public class Computer
    {
        public Computer(string fileName)
        {
            m_core = new List<int>();
            ReadFromFile(fileName);
        }

        public Computer(IEnumerable<int> codes)
        {
            Clear();
            m_core = codes.ToList();
        }

        private void ReadFromFile(string fileName)
        {
            Clear();
            var file = new StreamReader(fileName);
            while (!file.EndOfStream)
            {
                var line = file.ReadLine();
                var lineCodes = line.Trim().Split(',').Select(s => s.Trim());
                AppendCodes(lineCodes);
            }
            file.Close();
        }

        private void Clear()
        {
            m_core.Clear();
        }

        private void AppendCodes(IEnumerable<string> codes)
        {
            int pos = 0;
            try
            {
                foreach (var code in codes)
                {
                    var numCode = int.Parse(code);
                    pos++;
                    m_core.Append(numCode);
                }
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Error reading Computer codes from file: Non-numeric code at position {pos}");
                throw e;
            }
        }

        private List<int> m_core;
    }
}
