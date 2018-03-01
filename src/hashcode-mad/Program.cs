using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

            var vehicles = simulation.Run();

            var outputBuilder = new OutputBuilder(vehicles);
            outputBuilder.Build(@"output\" + Path.GetFileName(inputs[index]).Replace(".in", ".out"));

            Console.Read();
        }

        private static Simulation GetSimulation(string[] lines)
        {
            var values = lines[0].Split(' ')
                .Select(value => int.Parse(value))
                .ToArray();

            return new Simulation(rows: values[0], columns: values[1], vehicles: values[2], rides: values[3], bonus: values[4], steps: values[5]);
        }

        private static IEnumerable<Ride> GetRides(string[] lines)
        {
            for (var i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(' ')
                    .Select(value => int.Parse(value))
                    .ToArray();

                yield return new Ride(id: i, startX: values[0], startY: values[1], endX: values[2], endY: values[3], start: values[4], end: values[5]);
            }
        }
    }
}
