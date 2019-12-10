using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc2019
{
    public class Computer
    {
        public Computer(string fileName)
        {
            m_core = new List<int>();
            m_ip = 0;
            ReadFromFile(fileName);
        }

        public Computer(IEnumerable<int> codes)
        {
            m_core = codes.ToList();
            m_ip = 0;
        }

        /*
            Execute a single instruction at the current instruction pointer, m_ip.

            Returns:
            
            1 if correctly executed, 
            0 if program terminated normally,
            -1 or other value if something went wrong.
         */
        public int ExecuteInstruction()
        {
            var instruction = Read(m_ip++);
            int retcode;
            int arg1, arg2, destination, result;
            switch (instruction)
            {
                case 1:
                    // Add two arguments and store in a third position.
                    (arg1, arg2) = GrabTwoArguments(m_ip);
                    m_ip += 2;
                    result = arg1 + arg2;
                    destination = Read(m_ip++);
                    Write(destination, result);
                    retcode = 1;
                    break;
                case 2:
                    // Multiply two arguments and store in a third position.
                    (arg1, arg2) = GrabTwoArguments(m_ip);
                    m_ip += 2;
                    result = arg1 * arg2;
                    destination = Read(m_ip++);
                    Write(destination, result);
                    retcode = 1;
                    break;
                case 99:
                    // Normal program termination - return 0.
                    retcode = 0;
                    break;
                default:
                    throw new Exception($"Error executing instruction {instruction} at position {m_ip - 1}. Unrecognised opcode.");
            }

            return retcode;
        }

        public int ExecuteProgram(int pos)
        {
            m_ip = pos;
            var retcode = 1;
            while (retcode == 1)
            {
                retcode = ExecuteInstruction();
            }

            if (retcode == 0)
            {
                Console.WriteLine("Program terminated normally [0]");
            }
            else
            {
                Console.WriteLine($"Error executing program [{retcode}]");
            }

            return retcode;
        }

        private (int, int) GrabTwoArguments(int pos)
        {
            int addr1, addr2;
            (addr1, addr2) = GrabTwoAddresses(pos);
            var arg1 = Read(addr1);
            var arg2 = Read(addr2);
            return (arg1, arg2);
        }

        private (int, int) GrabTwoAddresses(int pos)
        {
            var arg1 = Read(pos++);
            var arg2 = Read(pos);
            return (arg1, arg2);
        }

        private void ReadFromFile(string fileName)
        {
            Clear();
            var file = new StreamReader(fileName);
            while (!file.EndOfStream)
            {
                var line = file.ReadLine();
                var lineCodes = line?.Trim().Split(',').Select(s => s.Trim());
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
            var pos = 0;
            try
            {
                foreach (var code in codes)
                {
                    var numCode = int.Parse(code);
                    pos++;
                    m_core.Add(numCode);
                }
            }
            catch (FormatException)
            {
                Console.WriteLine($"Error reading Computer codes from file: Non-numeric code at position {pos}");
                throw;
            }
        }

        private int Read(int pos)
        {
            try
            {
                var value = m_core[pos];
                return value;
            }
            catch (Exception)
            {
                throw new Exception($"Runtime error reading position {pos} in Computer.Read()");
            }
        }

        private void Write(int pos, int value)
        {
            try
            {
                m_core[pos] = value;
            }
            catch (Exception)
            {
                throw new Exception($"Runtime error writing position {pos} in Computer.Write()");
            }
        }

        private List<int> m_core;

        private int m_ip;

        public string StateString()
        {
            return string.Join(',', m_core.Select(i => i.ToString()));
        }

        public void Prime(int value1, int value2)
        {
            Write(1, value1);
            Write(2, value2);
        }

        public int ReadOut()
        {
            return Read(0);
        }

        public List<int> SaveState()
        {
            return m_core.ToList();
        }

        public void LoadState(List<int> state)
        {
            m_core.Clear();
            m_core.AddRange(state);
        }
    }
}
