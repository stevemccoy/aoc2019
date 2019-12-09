using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;
using aoc2019;

namespace Tests
{
    public class ModuleFuelTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(12, 2)]
        [TestCase(14, 2)]
        [TestCase(1969, 654)]
        [TestCase(100756, 33583)]
        public void ModuleFuelTest(int mass, int expectedFuel)
        {
            var actualFuel = aoc2019.Program.FuelRequired(mass);
            Assert.AreEqual(expectedFuel, actualFuel);
        }

        [TestCase(14,2)]
        [TestCase(1969, 966)]
        [TestCase(100756, 50346)]
        public void FullyFuelRequiredTest(int moduleMass, int expectedFuel)
        {
            var actualFuel = aoc2019.Program.FullyFuelRequired(moduleMass);
            Assert.AreEqual(expectedFuel, actualFuel);
        }

        [TestCase("1,0,0,3,99")]
        [TestCase("1,9,10,3,2,3,11,0,99,30,40,50")]
        public void ValidComputerProgramTest(string code)
        {
            var program = code.Split(',').Select(s => int.Parse(s));
            var computer = new Computer(program);
            int retCode = 0;
            try
            {
                retCode = computer.ExecuteProgram(0);
            }
            catch (MultipleAssertException e)
            {
                Assert.Fail($"Program execution failed abnormally {e.ToString()}");
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
            int retCode = 0;
            try
            {
                retCode = computer.ExecuteProgram(0);
            }
            catch (MultipleAssertException e)
            {
                Assert.Fail($"Program execution failed abnormally {e.ToString()}");
            }
            Assert.True(retCode == 0, $"Program terminated with nonzero return value {retCode}");
            Assert.AreEqual(end, computer.StateString());
        }

    }
}