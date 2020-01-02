using aoc2019;
using NUnit.Framework;

namespace aoc2019_tests
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
            var actualFuel = Program.FuelRequired(mass);
            Assert.AreEqual(expectedFuel, actualFuel);
        }

        [TestCase(14,2)]
        [TestCase(1969, 966)]
        [TestCase(100756, 50346)]
        public void FullyFuelRequiredTest(int moduleMass, int expectedFuel)
        {
            var actualFuel = Program.FullyFuelRequired(moduleMass);
            Assert.AreEqual(expectedFuel, actualFuel);
        }


    }
}