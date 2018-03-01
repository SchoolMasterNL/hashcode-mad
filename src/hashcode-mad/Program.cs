using System;
using System.Collections.Generic;
using System.IO;

namespace hashcode_mad
{
    class Program
    {
        private static readonly string[] inputs = new[] { @"input\a_example.in", @"input\b_should_be_easy.in", @"input\c_no_hurry.in", @"input\d_metropolis.in", @"input\e_high_bonus.in" };

        static void Main(string[] args)
        {
            Console.WriteLine("Which input:");

            for (var i = 0; i < inputs.Length; i++)
            {
                Console.WriteLine($"{i}: {inputs[i]}");
            }

            var key = Console.ReadKey();

            Console.WriteLine();

            var index = (int)key.Key - 48;

            var lines = File.ReadAllLines(inputs[index]);

            var simulation = GetSimulation(lines);

            Console.WriteLine(simulation);

            foreach (var ride in GetRides(lines))
            {
                Console.WriteLine(ride.ToString());
            }

            Console.Read();
        }

        private static Simulation GetSimulation(string[] lines)
        {
            var line = lines[0].Split(' ');

            return new Simulation(rows: int.Parse(line[0]), columns: int.Parse(line[1]), vehicles: int.Parse(line[2]), rides: int.Parse(line[3]), bonus: int.Parse(line[4]), steps: int.Parse(line[5]));
        }

        private static IEnumerable<Ride> GetRides(string[] lines)
        {
            for (var i = 1; i < lines.Length; i++)
            {
                var line = lines[i].Split(' ');

                yield return new Ride(id: i, startX: int.Parse(line[0]), startY: int.Parse(line[1]), endX: int.Parse(line[2]), endY: int.Parse(line[3]), start: int.Parse(line[4]), end: int.Parse(line[5]));
            }
        }
    }
}
