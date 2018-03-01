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
            this.vehicles = vehicles.Where(v => v.Rides.Count() > 0).ToList();
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

                        var rides = vehicles[i].Rides.ToList();
                        writer.Write(rides.Count);
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
