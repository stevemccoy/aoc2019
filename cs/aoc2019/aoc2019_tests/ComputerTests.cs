using System.Linq;
using aoc2019;
using NUnit.Framework;

namespace aoc2019_tests
{
    class ComputerTests
    {
        [TestCase("1,0,0,3,99")]
        [TestCase("1,9,10,3,2,3,11,0,99,30,40,50")]
        [TestCase("1101,100,-1,4,0")]
        public void ValidComputerProgramTest(string code)
        {
            var program = code.Split(',').Select(s => int.Parse(s));
            var computer = new Computer(program);
            var retCode = 0;
            try
            {
                retCode = computer.ExecuteProgram(0);
            }
            catch (MultipleAssertException e)
            {
                Assert.Fail($"Program execution failed abnormally {e}");
            }
            Assert.True(retCode == 0, $"Program terminated with nonzero return value {retCode}");
        }


        [TestCase("1,0,0,0,99", "2,0,0,0,99")]
        [TestCase("2,3,0,3,99", "2,3,0,6,99")]
        [TestCase("2,4,4,5,99,0", "2,4,4,5,99,9801")]
        [TestCase("1,1,1,4,99,5,6,0,99", "30,1,1,4,2,5,6,0,99")]
        public void ComputerStateChangeTest(string start, string end)
        {
            var program = start.Split(',').Select(s => int.Parse(s));
            var computer = new Computer(program);
            var retCode = 0;
            try
            {
                retCode = computer.ExecuteProgram(0);
            }
            catch (MultipleAssertException e)
            {
                Assert.Fail($"Program execution failed abnormally {e}");
            }
            Assert.True(retCode == 0, $"Program terminated with nonzero return value {retCode}");
            Assert.AreEqual(end, computer.StateString());
        }

        private const string BigMachine =
            @"3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106 0,36,98,0,0"
            + ",1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20"
            + ",1105,1,46,98,99";

        [TestCase("3,9,8,9,10,9,4,9,99,-1,8", 7, 0)]
        [TestCase("3,9,8,9,10,9,4,9,99,-1,8", 8, 1)]
        [TestCase("3,9,7,9,10,9,4,9,99,-1,8", 7, 1)]
        [TestCase("3,9,7,9,10,9,4,9,99,-1,8", 8, 0)]
        [TestCase("3,3,1108,-1,8,3,4,3,99", 8, 1)]
        [TestCase("3,3,1108,-1,8,3,4,3,99", 0, 0)]
        [TestCase("3,3,1107,-1,8,3,4,3,99", -1, 1)]
        [TestCase("3,3,1107,-1,8,3,4,3,99", 100, 0)]
        [TestCase("3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9", 0, 0)]
        [TestCase("3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9", -1, 1)]
        [TestCase("3,3,1105,-1,9,1101,0,0,12,4,12,99,1", 0, 0)]
        [TestCase("3,3,1105,-1,9,1101,0,0,12,4,12,99,1", 1, 1)]
        [TestCase("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,\r\n1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,\r\n999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99", 7, 999)]
        [TestCase("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,\r\n1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,\r\n999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99", 8, 1000)]
        [TestCase("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,\r\n1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,\r\n999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99", 9, 1001)]
        public void ComputerInputOutputTest(string start, int input, int output)
        {
            var program = start.Split(',').Select(s => int.Parse(s.Trim()));
            var computer = new Computer(program);
            computer.InputQueue.Enqueue(input);
            var retCode = 0;
            try
            {
                retCode = computer.ExecuteProgram(0);
            }
            catch (MultipleAssertException e)
            {
                Assert.Fail($"Program execution failed abnormally {e}");
            }
            Assert.True(retCode == 0, $"Program terminated with nonzero return value {retCode}");
            Assert.AreEqual(1, computer.OutputQueue.Count);
            var actualOutput = computer.OutputQueue.Dequeue();
            Assert.AreEqual(output, actualOutput);
        }
    }
}
