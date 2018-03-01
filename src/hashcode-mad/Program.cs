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

            foreach (var ride in GetRides(inputs[index]))
            {
                Console.WriteLine(ride.ToString());
            }

            Console.Read();
        }

        private static IEnumerable<Ride> GetRides(string path)
        {
            var lines = File.ReadAllLines(path);

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Split(' ');

                yield return new Ride
                {
                    Id = i,
                    StartX = int.Parse(line[0]),
                    StartY = int.Parse(line[1]),
                    EndX = int.Parse(line[2]),
                    EndY = int.Parse(line[3]),
                    Start = int.Parse(line[4]),
                    End = int.Parse(line[5])
                };
            }
        }
    }
}
