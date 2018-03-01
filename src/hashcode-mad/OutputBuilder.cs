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
                    foreach (var vehicle in vehicles)
                    {
                        WriteVehicle(writer, vehicle);
                    }
                }
            }
        }

        private static void WriteVehicle(StreamWriter writer, Vehicle vehicle)
        {
            writer.Write(vehicle.Id);
            foreach (var ride in vehicle.Rides)
            {
                writer.Write(" " + ride.Id);
            }
            writer.WriteLine();
        }
    }
}
