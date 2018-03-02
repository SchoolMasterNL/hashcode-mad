using System.Collections.Generic;
using System.Linq;

namespace hashcode_mad
{
    internal class Simulation
    {
        public Simulation(int rows, int columns, int vehicles, int rides, int bonus, int steps)
        {
            this.Rows = rows;
            this.Columns = columns;
            this.Vehicles = vehicles;
            this.Rides = rides;
            this.Bonus = bonus;
            this.Steps = steps;
        }

        public int Rows { get; }

        public int Columns { get; }

        public int Vehicles { get; }

        public int Rides { get; }

        public int Bonus { get; }

        public int Steps { get; }

        public IEnumerable<Vehicle> Run1(IEnumerable<Ride> input)
        {
            var result = Enumerable.Range(0, this.Vehicles).Select(index => new Vehicle(index, this.Bonus)).ToList();
            var rides = input.ToList();

            foreach (var vehicle in result)
            {
                foreach (var ride in rides)
                {
                    var bestRide = (Ride)null;

                    // Can we finish in time?
                    var totalTime = vehicle.GetDistance(ride) + ride.Distance;
                    if ((totalTime + vehicle.CurrentStep) >= ride.End)
                    {
                        continue;
                    }

                    // Longer ride?
                    if (bestRide == null || ride.Distance > bestRide.Distance)
                    {
                        bestRide = ride;
                    }
                    else if (ride.Distance == bestRide.Distance)
                    {
                        // Can we start in time?
                        if ((vehicle.CurrentStep + vehicle.GetDistance(ride)) == ride.Start)
                        {
                            // We can make it in time! Bonus points
                            bestRide = ride;
                        }
                    }

                    vehicle.AssignRide(bestRide);
                }

                rides = rides.Except(vehicle.Rides).ToList();
            }

            return result;
        }

        public IEnumerable<Vehicle> Run2(IEnumerable<Ride> input)
        {
            var vehicles = Enumerable.Range(0, this.Vehicles).Select(index => new Vehicle(index, this.Bonus)).ToList();
            var rides = input.ToList();

            foreach (var ride in rides)
            {
                // Short the vehicles by closest to this ride.
                var bestVehicles = vehicles.OrderBy(v => v.GetDistance(ride));
                foreach (var vehicle in bestVehicles)
                {
                    int total = vehicle.CurrentStep + (vehicle.GetDistance(ride) + ride.Distance);
                    if (total >= ride.End)
                        continue;

                    vehicle.AssignRide(ride);
                    break;
                }

            }
            return vehicles;
        }

        public override string ToString()
        {
            return $"Rows: {Rows}, Columns: {Columns}, Vehicles: {Vehicles}, Rides: {Rides}, Bonus: {Bonus}, Steps: {Steps}";
        }
    }
}
