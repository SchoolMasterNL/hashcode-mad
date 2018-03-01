using System.Collections.Generic;
using System.IO;

namespace hashcode_mad
{
    internal class OutputBuilder
    {
        private readonly IEnumerable<Vehicle> vehicles;

        public OutputBuilder(IEnumerable<Vehicle> vehicles)
        {
            this.vehicles = vehicles;
        }

        public void Build(string fileName)
        {
            using (var output = File.OpenWrite(fileName))
            {
                using (var writer = new StreamWriter(output))
                {
                    int index = 0;
                    foreach (var vehicle in vehicles)
                    {
                        writer.Write(++index);
                        foreach (var ride in vehicle.Rides)
                        {
                            writer.Write(" " + ride.Id);
                        }
                        writer.WriteLine();
                    }
                }
            }
        }
    }
}
