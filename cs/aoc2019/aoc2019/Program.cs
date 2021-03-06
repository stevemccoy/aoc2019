﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc2019
{
    public class Program
    {
        private const string InputFile1 = @"C:\src\github\aoc2019\cs\aoc2019\aoc2019\input\input1.txt";
        private const string InputFile2 = @"C:\src\github\aoc2019\cs\aoc2019\aoc2019\input\input2.txt";
        private const string InputFile3 = @"C:\src\github\aoc2019\cs\aoc2019\aoc2019\input\input3.txt";
        private const string InputFile5 = @"C:\src\github\aoc2019\cs\aoc2019\aoc2019\input\input5.txt";
        private const string InputFile7 = @"C:\src\github\aoc2019\cs\aoc2019\aoc2019\input\input7.txt";
        private const string InputFile8 = @"C:\src\github\aoc2019\cs\aoc2019\aoc2019\input\input8.txt";

        static void Main()
        {
            Console.WriteLine("Advent of Code 2019");

            /*
                        Day1Part1();
                        Day1Part2();
                        Day2Part1();
                        Day2Part2();
                        Day3();
                        Day5Part1();
                        Day5Part2();

                        Day7Part1();
                        Day7Part2();
            */
            Day8Part1();

        }

        private static void Day8Part1()
        {
            Console.WriteLine("Day 8, Part 1");
            var charsPerFrame = 25 * 6;
            var chars = string.Join("", File.ReadAllLines(InputFile8)).Where(char.IsDigit).ToArray();
            var numChars = chars.Length;
            var offset = 0;
            List<Tuple<int, int, int>> results = new List<Tuple<int, int, int>>();
            var frame = new Char[charsPerFrame];
            var minZeroes = int.MaxValue;
            var indexMinZeroes = int.MaxValue;
            var index = 0;

            while (offset < numChars)
            {
                Array.Copy(chars, offset, frame, 0, charsPerFrame);
                var zeroes = frame.Count(c => c.Equals('0'));
                var ones = frame.Count(c => c.Equals('1'));
                var twos = frame.Count(c => c.Equals('2'));
                var t = new Tuple<int, int, int>(zeroes, ones, twos);
                results.Add(t);
                offset += charsPerFrame;
                if (zeroes < minZeroes)
                {
                    minZeroes = zeroes;
                    indexMinZeroes = index;
                }

                index++;
            }

            int product;
            foreach (var result in results)
            {
                product = result.Item2 * result.Item3;
                Console.WriteLine($"Zeroes:{result.Item1}, Ones:{result.Item2}, Twos:{result.Item3}, Product:{product}");
            }

            product = results[indexMinZeroes].Item2 * results[indexMinZeroes].Item3;
            Console.WriteLine($"Minimum zeroes for index: {indexMinZeroes}, with product {product}");

            Console.WriteLine($"No chars {numChars}");
            Console.Write("Hit return to quit");
            Console.ReadLine();
        }

        private static void TestPermutations()
        {
            var options = new List<int>() {1, 2, 3, 4, 5, 6};
            var permutations = Permutations(new List<int>(), options).ToList();
            foreach (var option in permutations)
            {
                foreach (var i in option)
                {
                    Console.Write(i);
                }
                Console.WriteLine();
            }
            Console.WriteLine($"Number of permutations: {permutations.Count()}");
        }

        private static void Day1Part1()
        {
            Console.WriteLine("Day 1, Part 1:\n");
            var moduleMasses = ReadInputMasses();
            var fuelTotal = 0;
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

        private static int[] ReadInputMasses()
        {
            var lines = File.ReadAllLines(InputFile1);
            var masses = lines.Select(l => int.Parse(l.Trim()));
            return masses.ToArray();
        }

        public static int FuelRequired(int mass)
        {
            var fuel = (mass / 3) - 2;
            return (fuel > 0 ? fuel : 0);
        }

        private static void Day1Part2()
        {
            Console.WriteLine("Day 1, Part 2:\n");
            var moduleMasses = ReadInputMasses();

            var fuelTotal = moduleMasses.Sum(m => FullyFuelRequired(m));
            Console.WriteLine($"Total fuel required = {fuelTotal}");
            Console.Write("Hit return to quit");
            Console.ReadLine();
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

        private static void Day2Part1()
        {
            Console.WriteLine("Day 2, Part 1:\n");
            var computer = new Computer(InputFile2);

            computer.Prime(12, 2);
            computer.ExecuteProgram(0);
            var result = computer.ReadOut();

            Console.WriteLine($"Result = {result}");
            Console.Write("Hit return to quit");
            Console.ReadLine();
        }

        private static void Day2Part2()
        {
            const int wantedResult = 19690720;
            var computer = new Computer(InputFile2);
            var initialState = computer.SaveState();
            var done = false;

            for (var i = 0; i < 100; i++)
            {
                for (var j = 0; j < 100; j++)
                {
                    computer.LoadState(initialState);
                    computer.Prime(i, j);
                    computer.ExecuteProgram(0);
                    var result = computer.ReadOut();
                    Console.WriteLine($"Input: ({i},{j}), Output: {result}");
                    if (result == wantedResult)
                    {
                        Console.WriteLine("Wanted result achieved!");
                        done = true;
                        break;
                    }
                }
                if (done)
                {
                    break;
                }
            }
            Console.Write("Hit return to quit");
            Console.ReadLine();
        }

        private static void Day3()
        {
            var lines = File.ReadAllLines(InputFile3);
            var paths = new List<Path>();
            var id = 1;
            var intersections = new Dictionary<(int,int), HashSet<(int, int)>>();
            var crossings = new Dictionary<(int, int), int>();

            foreach (var line in lines)
            {
                var path = new Path(line, id++);
                paths.Add(path);
                path.TraceMoves(id);
            }

            foreach (var firstPath in paths)
            {
                foreach (var secondPath in paths)
                {
                    if (secondPath.Id > firstPath.Id)
                    {
                        var cross = Intersections(firstPath, secondPath);
                        if (cross.Count > 0)
                        {
                            intersections[(firstPath.Id, secondPath.Id)] = cross.Keys.ToHashSet();
                            foreach (var (key, value) in cross)
                            {
                                crossings[key] = value;
                            }
                        }
                    }
                }
            }

            int minDistance = int.MaxValue;
            (int, int) closestTuple = (int.MaxValue, int.MaxValue);
            foreach (var item in intersections.Values)
            {
                foreach (var p in item)
                {
                    var d = Manhattan((0, 0), (p.Item1, p.Item2));
                    if (d < minDistance)
                    {
                        minDistance = d;
                        closestTuple = p;
                    }
                }
            }

            Console.WriteLine($"Part 1: Closest Tuple at distance {minDistance}, {closestTuple}");

            // Part 2 stuff.

            minDistance = int.MaxValue;
            var minCrossing = (int.MaxValue, int.MaxValue);

            foreach (var crossing in crossings)
            {
                if (crossing.Value < minDistance)
                {
                    minDistance = crossing.Value;
                    minCrossing = crossing.Key;
                }
            }

            Console.WriteLine($"Part 2: Closest crossing at distance {minDistance}, location: {minCrossing}.");

        }

        private static Dictionary<(int, int), int> Intersections(Path firstPath, Path secondPath)
        {
            var result = new Dictionary<(int, int), int>();
            foreach (var p in firstPath.Seen.Keys)
            {
                if (secondPath.Seen.ContainsKey(p))
                {
                    result[p] = firstPath.Seen[p] + secondPath.Seen[p];
                }
            }

            return result;
        }

        private static int Manhattan((int, int) point1, (int, int) point2)
        {
            var d = Math.Abs(point1.Item1 - point2.Item1);
            d += Math.Abs(point1.Item2 - point2.Item2);
            return d;
        }

        private static void Day5Part1()
        {
            var computer = new Computer(InputFile5);
            // Set up user input of 1.
            computer.InputQueue.Enqueue(1);
            // Run program.
            computer.ExecuteProgram(0);
            // Read output.
            Console.WriteLine($"Day 5, Part 1. Outputs:");
            while (computer.OutputQueue.Count > 0)
            {
                Console.WriteLine(computer.OutputQueue.Dequeue());
            }

            Console.Write("Hit return to quit");
            Console.ReadLine();
        }

        private static void Day5Part2()
        {
            var computer = new Computer(InputFile5);
            // Set up user input of 1.
            computer.InputQueue.Enqueue(5);
            // Run program.
            computer.ExecuteProgram(0);
            // Read output.
            Console.WriteLine($"Day 5, Part 2. Outputs:");
            while (computer.OutputQueue.Count > 0)
            {
                Console.WriteLine(computer.OutputQueue.Dequeue());
            }

            Console.Write("Hit return to quit");
            Console.ReadLine();
        }

        public static void Day7Part1()
        {
            Console.WriteLine("Day 7, Part 1");
            var actualMaxThrust = 0;
            var reader = new StreamReader(InputFile7);
            var program = reader.ReadToEnd();
            reader.Close();
            foreach (var phaseSettings in Permutations(new int[] { 0, 1, 2, 3, 4 }))
            {
                var bank = new AmplifierBank(program, phaseSettings);
                var thrust = bank.Run(false);
                if (thrust > actualMaxThrust)
                {
                    actualMaxThrust = thrust;
                }
            }

            Console.WriteLine("Analysis complete.");
            Console.WriteLine($"Maximum thrust = {actualMaxThrust}");
        }

        public static void Day7Part2()
        {
            Console.WriteLine("Day 7, Part 2");
            var actualMaxThrust = 0;
            var reader = new StreamReader(InputFile7);
            var program = reader.ReadToEnd();
            reader.Close();
            foreach (var phaseSettings in Permutations(new int[] {5, 6, 7, 8, 9}))
            {
                var bank = new AmplifierBank(program, phaseSettings);
                var thrust = bank.Run(true);
                if (thrust > actualMaxThrust)
                {
                    actualMaxThrust = thrust;
                }
            }

            Console.WriteLine("Analysis complete.");
            Console.WriteLine($"Maximum thrust = {actualMaxThrust}");
        }

        public static IEnumerable<List<T>> Permutations<T>(IEnumerable<T> options)
        {
            return Permutations(new List<T>(), options.ToList());
        }

        public static IEnumerable<List<T>> Permutations<T>(List<T> sofar, List<T> options)
        {
            if (options.Any())
            {
                var newOptions = new List<T>(options);
                foreach (var option in options)
                {
                    var path = new List<T>(sofar) {option};

                    newOptions.Remove(option);
                    foreach (var permutation in Permutations(path, newOptions))
                    {
                        yield return permutation;
                    }
                    newOptions.Add(option);
                }
            }
            else
            {
                yield return sofar;
            }
        }

    }
}
