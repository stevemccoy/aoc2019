using System;
using System.IO;
using System.Linq;

namespace aoc2019
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2019");

//            Day1Part1();
//            Day1Part2();
            Day2Part1();
            Day2Part2();
        }

        private static void Day1Part1()
        {
            Console.WriteLine("Day 1, Part 1:\n");
            int[] moduleMasses = ReadInputMasses();
            int fuelTotal = 0;
            foreach (var moduleMass in moduleMasses)
            {
                var moduleFuel = FuelRequired(moduleMass);
                Console.WriteLine($"Module mass: {moduleMass}, Fuel: {moduleFuel}");
                fuelTotal += moduleFuel;
            }
            Console.WriteLine($"Total fuel required = {fuelTotal}");
            Console.Write("Hit return to quit");
            Console.ReadLine();
        }

        private static void Day1Part2()
        {
            Console.WriteLine("Day 1, Part 2:\n");
            int[] moduleMasses = ReadInputMasses();

            var fuelTotal = moduleMasses.Sum(m => FullyFuelRequired(m));
            Console.WriteLine($"Total fuel required = {fuelTotal}");
            Console.Write("Hit return to quit");
            Console.ReadLine();
        }

        private const string InputFile2 = @"C:\src\github\aoc2019\cs\aoc2019\aoc2019\input\input2.txt";

        private static void Day2Part1()
        {
            Console.WriteLine("Day 2, Part 1:\n");
            var computer = new Computer(InputFile2);

            computer.Prime(12, 2);
            computer.ExecuteProgram();
            var result = computer.ReadOut();

            Console.WriteLine($"Result = {result}");
            Console.Write("Hit return to quit");
            Console.ReadLine();
        }

        private static void Day2Part2()
        {
            const int WantedResult = 19690720;
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    var computer = new Computer(InputFile2);
                    computer.Prime(i, j);
                    computer.ExecuteProgram(0);
                    var result = computer.ReadOut();
                    Console.WriteLine($"Input: ({i},{j}), Output: {result}");
                    if (result == WantedResult)
                    {
                        Console.WriteLine("Wanted result achieved!");
                        break;
                    }
                }
            }
            Console.Write("Hit return to quit");
            Console.ReadLine();
        }

        private static int[] ReadInputMasses()
        {
            string[] lines = File.ReadAllLines(@"C:\src\github\aoc2019\cs\aoc2019\aoc2019\input\input1.txt");
            var masses = lines.Select(l => int.Parse(l.Trim()));
            return masses.ToArray();
        }

        public static int FuelRequired(int mass)
        {
            var fuel = (mass / 3) - 2;
            return (fuel > 0 ? fuel : 0);
        }

        public static int FullyFuelRequired(int moduleMass)
        {
            var fuelRequired = FuelRequired(moduleMass);
            var fuelTotal = fuelRequired;
            while (fuelRequired > 0)
            {
                fuelRequired = FuelRequired(fuelRequired);
                fuelTotal += fuelRequired;
            }

            return fuelTotal;
        }
    }
}
