using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace hashcode_mad
{
    internal class OutputBuilder
    {
        private readonly List<Vehicle> vehicles;

        public OutputBuilder(IEnumerable<Vehicle> vehicles)
        {
            this.vehicles = vehicles.ToList();
        }

        public void Build(string fileName)
        {
            using (var output = File.Create(fileName))
            {
                using (var writer = new StreamWriter(output))
                {
                    for (int i = 0; i < vehicles.Count; i++)
                    {
                        if (i > 0)
                            writer.WriteLine();

                        writer.Write(vehicles[i].Id);
                        foreach (var ride in vehicles[i].Rides)
                        {
                            writer.Write(" " + ride.Id);
                        }
                    }
                }
            }
        }
    }
}
