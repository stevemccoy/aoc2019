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
            Initialize();
            ReadFromFile(fileName);
        }

        public Computer(IEnumerable<int> codes)
        {
            Initialize();
            mCore = codes.ToList();
            mIp = 0;
        }

        private void Initialize()
        {
            InputQueue = new Queue<int>();
            OutputQueue = new Queue<int>();
            mCore = new List<int>();
            mIp = 0;
        }

        public int ExecuteProgram(int pos)
        {
            mIp = pos;
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

        /*
            Execute a single instruction at the current instruction pointer, m_ip.

            Returns:
            
            1 if correctly executed, 
            0 if program terminated normally,
            -1 or other value if something went wrong.
         */
        private int ExecuteInstruction()
        {
            var instruction = Read(mIp++);
            // Base opcode of the instruction.
            int opcode = instruction % 100;
            // Parameter modes for this instruction.
            int pmodeArg1 = (instruction / 100) % 10;
            int pmodeArg2 = (instruction / 1000) % 10;

            int retcode = 1;
            int arg1, arg2, destination, result;
            switch (opcode)
            {
                case 1:
                    // Add two arguments and store in a third position.
                    (arg1, arg2) = GrabTwoArguments(mIp, pmodeArg1, pmodeArg2);
                    mIp += 2;
                    result = arg1 + arg2;
                    destination = Read(mIp++);
                    Write(destination, result);
                    break;
                case 2:
                    // Multiply two arguments and store in a third position.
                    (arg1, arg2) = GrabTwoArguments(mIp, pmodeArg1, pmodeArg2);
                    mIp += 2;
                    result = arg1 * arg2;
                    destination = Read(mIp++);
                    Write(destination, result);
                    break;
                case 3:
                    // Input to a location in memory.
                    result = InputQueue.Dequeue();
                    destination = Read(mIp++);
                    if (pmodeArg1 != 0)
                    {
                        throw new Exception($"Illegal parameter mode {pmodeArg1} for opcode {opcode}.");
                    }
                    Write(destination, result);
                    break;
                case 4:
                    // Output from a location in memory.
                    arg1 = Read(mIp++);
                    result = (pmodeArg1 == 1) ? arg1 : Read(arg1);
                    OutputQueue.Enqueue(result);
                    break;
                case 5:
                    // Jump if true.
                    (arg1, arg2) = GrabTwoArguments(mIp, pmodeArg1, pmodeArg2);
                    mIp += 2;
                    if (arg1 != 0)
                    {
                        mIp = arg2;
                    }

                    break;
                case 6:
                    // Jump if false.
                    (arg1, arg2) = GrabTwoArguments(mIp, pmodeArg1, pmodeArg2);
                    mIp += 2;
                    if (arg1 == 0)
                    {
                        mIp = arg2;
                    }

                    break;
                case 7:
                    // Less than.
                    (arg1, arg2) = GrabTwoArguments(mIp, pmodeArg1, pmodeArg2);
                    mIp += 2;
                    destination = Read(mIp++);
                    Write(destination, (arg1 < arg2) ? 1 : 0);
                    break;
                case 8:
                    // Equals.
                    (arg1, arg2) = GrabTwoArguments(mIp, pmodeArg1, pmodeArg2);
                    mIp += 2;
                    destination = Read(mIp++);
                    Write(destination, (arg1 == arg2) ? 1 : 0);
                    break;

                case 99:
                    // Normal program termination - return 0.
                    retcode = 0;
                    break;
                default:
                    throw new Exception($"Error executing instruction {instruction} at position {mIp - 1}. Unrecognised opcode.");
            }

            return retcode;
        }

        private (int, int) GrabTwoArguments(int pos, int pmode1, int pmode2)
        {
            var arg1 = GrabInputArgument(pos++, pmode1);
            var arg2 = GrabInputArgument(pos, pmode2);
            return (arg1, arg2);
        }

        private int GrabInputArgument(int pos, int pmode)
        {
            var arg = Read(pos);
            switch (pmode)
            {
                case 0:
                    // Indirect mode.
                    return Read(arg);
                case 1:
                    // Direct mode.
                    return arg;
                default:
                    Console.WriteLine($"Unrecognised parameter mode {pmode} at position {pos}");
                    return -1;
            }
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
            mCore.Clear();
            InputQueue.Clear();
            OutputQueue.Clear();
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
                    mCore.Add(numCode);
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
                var value = mCore[pos];
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
                mCore[pos] = value;
            }
            catch (Exception)
            {
                throw new Exception($"Runtime error writing position {pos} in Computer.Write()");
            }
        }

        private List<int> mCore;

        private int mIp;

        public Queue<int> InputQueue { get; set; }
        public Queue<int> OutputQueue { get; set; }

        public string StateString()
        {
            return string.Join(',', mCore.Select(i => i.ToString()));
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
            return mCore.ToList();
        }

        public void LoadState(List<int> state)
        {
            mCore.Clear();
            mCore.AddRange(state);
        }
    }
}
