using System;
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

        public (IEnumerable<Vehicle>, int) Run(IEnumerable<Ride> input)
        {
            var runs = new Func<IEnumerable<Ride>, IEnumerable<Vehicle>>[]
            {
                Run1,
                Run2,
                Run3,
                Run4,
                Run5,
                Run6,
                Run7,
                Run8,
                Run9,
            };

            var bestVehicles = (IEnumerable<Vehicle>)null;
            var bestTotal = 0;

            for (int i = 0; i < runs.Length; i++)
            {
                var rides = input.ToList();
                var vehicles = runs[i](rides);

                var score = vehicles.Sum(v => v.Score);
                var bonus = vehicles.Sum(v => v.Bonus);
                var remaining = rides.Count - vehicles.Sum(v => v.Rides.Count());
                var total = score + bonus;

                Console.WriteLine($"Run{i + 1}= Score: {score:n0}, Bonus {bonus:n0}, Total: {total:n0}, Remaining: {remaining}");
                if (total >= bestTotal)
                {
                    bestVehicles = vehicles;
                    bestTotal = total;
                }
            }

            return (bestVehicles, bestTotal);
        }

        private IEnumerable<Vehicle> Run1(IEnumerable<Ride> input)
        {
            var result = Enumerable.Range(0, this.Vehicles).Select(index => new Vehicle(index, this.Bonus)).ToList();
            var rides = input.ToList();

            foreach (var vehicle in result)
            {
                foreach (var ride in rides)
                {
                    var bestRide = (Ride)null;

                    if (!vehicle.CanFinish(ride))
                        continue;

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

        private IEnumerable<Vehicle> Run2(IEnumerable<Ride> input)
        {
            var vehicles = Enumerable.Range(0, this.Vehicles).Select(index => new Vehicle(index, this.Bonus)).ToList();
            var rides = input.ToList();

            foreach (var ride in rides)
            {
                // Short the vehicles by closest to this ride.
                var bestVehicles = vehicles.OrderBy(v => v.GetDistance(ride));
                foreach (var vehicle in bestVehicles)
                {
                    if (!vehicle.CanFinish(ride))
                        continue;

                    vehicle.AssignRide(ride);
                    break;
                }

            }
            return vehicles;
        }

        private IEnumerable<Vehicle> Run3(IEnumerable<Ride> input)
        {
            var vehicles = Enumerable.Range(0, this.Vehicles).Select(index => new Vehicle(index, this.Bonus)).ToList();
            var rides = input.ToList();

            foreach (var ride in rides)
            {
                // Sort the vehicles by closest to this ride.
                var bestVehicles = vehicles.OrderBy(v => v.GetDistance(ride));
                var bestVehicle = (Vehicle)null;
                var bestScore = 0;

                foreach (var vehicle in bestVehicles)
                {
                    if (!vehicle.CanFinish(ride))
                        continue;

                    var score = vehicle.GetScoreAndBonus(ride);
                    if (score > bestScore)
                    {
                        bestVehicle = vehicle;
                        bestScore = score;
                    }
                }

                if (bestVehicle != null)
                {
                    bestVehicle.AssignRide(ride);
                }
            }
            return vehicles;
        }

        private IEnumerable<Vehicle> Run4(IEnumerable<Ride> input)
        {
            var result = Enumerable.Range(0, this.Vehicles).Select(index => new Vehicle(index, this.Bonus)).ToList();
            var rides = input.ToList();

            foreach (var ride in rides)
            {
                var vehicles = result.Where(_ => _.CanFinish(ride));
                if (vehicles.Count() == 0)
                    continue;

                var bestVehicle = vehicles.OrderBy(_ => _.GetRideScore(ride)).First();
                bestVehicle.AssignRide(ride);
            }

            return result;
        }

        private IEnumerable<Vehicle> Run5(IEnumerable<Ride> input)
        {
            var result = Enumerable.Range(0, this.Vehicles).Select(index => new Vehicle(index, this.Bonus)).ToList();
            var rides = input.ToList();

            while (rides.Count > 0)
            {
                var done = true;
                foreach (var vehicle in result)
                {
                    var vehicleRides = rides.Where(_ => vehicle.CanFinish(_));
                    if (vehicleRides.Count() == 0)
                        continue;

                    var bestRide = vehicleRides.OrderBy(_ => vehicle.GetRideScore(_)).First();
                    vehicle.AssignRide(bestRide);
                    rides.Remove(bestRide);
                    done = false;
                }

                if (done)
                    break;
            }

            return result;
        }

        private IEnumerable<Vehicle> Run6(IEnumerable<Ride> input)
        {
            var result = Enumerable.Range(0, this.Vehicles).Select(index => new Vehicle(index, this.Bonus)).ToList();
            var rides = input.ToList();

            while (rides.Count > 0)
            {
                var done = true;
                foreach (var vehicle in result)
                {
                    var vehicleRides = rides.Where(_ => vehicle.CanFinish(_));
                    if (vehicleRides.Count() == 0)
                        continue;

                    var bestRide = vehicleRides.OrderByDescending(_ => _.Distance - vehicle.GetRideScore(_)).First();
                    vehicle.AssignRide(bestRide);
                    rides.Remove(bestRide);
                    done = false;
                }

                if (done)
                    break;
            }

            return result;
        }

        private IEnumerable<Vehicle> Run7(IEnumerable<Ride> input)
        {
            var vehicles = Enumerable.Range(0, this.Vehicles).Select(index => new Vehicle(index, this.Bonus)).ToList();
            var rides = input.OrderBy(_ => _.Start).ToList();

            foreach (var vehicle in vehicles)
            {
                var ride = rides.Where(_ => vehicle.CanFinish(_)).OrderByDescending(_ => vehicle.GetRideScore2(_)).FirstOrDefault();
                while (ride != null)
                {
                    vehicle.AssignRide(ride);
                    rides.Remove(ride);

                    ride = rides.Where(_ => vehicle.CanFinish(_)).OrderByDescending(_ => vehicle.GetRideScore2(_)).FirstOrDefault();
                }
            }

            return vehicles;
        }

        private IEnumerable<Vehicle> Run8(IEnumerable<Ride> input)
        {
            var vehicles = Enumerable.Range(0, this.Vehicles).Select(index => new Vehicle(index, this.Bonus)).ToList();
            var rides = input.OrderBy(_ => _.Start).ToList();

            foreach (var vehicle in vehicles)
            {
                var ride = rides.Where(_ => vehicle.CanFinish(_)).OrderByDescending(_ => (Steps - _.Start) + vehicle.GetRideScore2(_)).FirstOrDefault();
                while (ride != null)
                {
                    vehicle.AssignRide(ride);
                    rides.Remove(ride);

                    ride = rides.Where(_ => vehicle.CanFinish(_)).OrderByDescending(_ => (Steps - _.Start) + vehicle.GetRideScore2(_)).FirstOrDefault();
                }
            }

            return vehicles;
        }

        private IEnumerable<Vehicle> Run9(IEnumerable<Ride> input)
        {
            List<Vehicle> TryReverseRides(int count)
            {
                var result = new List<Vehicle>();
                var rides = input.ToList();

                var startAtEnd = count;

                var reverseVehicles = Enumerable.Range(0, startAtEnd).Select(index => new ReverseVehicle(index, this.Bonus)).ToList();
                var vehicles = Enumerable.Range(0, Vehicles - startAtEnd).Select(index => new Vehicle(index + startAtEnd, this.Bonus)).ToList();

                var bestEnds = rides.OrderByDescending(_ => _.End + _.Distance).ToList();

                var offset = 0;
                foreach (var reverseVehicle in reverseVehicles)
                {
                    while (offset < bestEnds.Count)
                    {
                        var ride = bestEnds[offset];

                        if (ride.GetDistance(0, 0) + ride.Distance < ride.End)
                            break;

                        offset++;
                    }

                    if (offset == bestEnds.Count)
                        break;

                    reverseVehicle.SetEnd(bestEnds[offset]);

                    rides.Remove(bestEnds[offset]);

                    offset++;
                }

                if (reverseVehicles.Any(_ => _.CurrentStep == 0))
                    throw new NotImplementedException();

                while (rides.Count > 0)
                {
                    var done = true;

                    foreach (var reverseVehicle in reverseVehicles)
                    {
                        var ride = rides.Where(_ => reverseVehicle.CanStart(_)).OrderByDescending(_ => _.End + _.Distance).FirstOrDefault();
                        while (ride != null)
                        {
                            reverseVehicle.AssignRide(ride);
                            rides.Remove(ride);
                            done = false;
                            ride = rides.Where(_ => reverseVehicle.CanStart(_)).OrderByDescending(_ => _.End + _.Distance).FirstOrDefault();
                        }
                    }

                    if (done)
                        break;
                }

                while (rides.Count > 0)
                {
                    var done = true;

                    foreach (var vehicle in vehicles)
                    {
                        var ride = rides.Where(_ => vehicle.CanFinish(_)).OrderByDescending(_ => (Steps - _.Start) + vehicle.GetRideScore3(_)).FirstOrDefault();
                        if (ride != null)
                        {
                            vehicle.AssignRide(ride);
                            rides.Remove(ride);
                            done = false;
                        }
                    }

                    if (done)
                        break;
                }

                return reverseVehicles.Select(_ => _.ToVehicle()).Concat(vehicles).ToList();
            }

            var bestVehicles = TryReverseRides(Vehicles);

            var max = Math.Max(Vehicles * 0.02, 1.0);
            for (int i = 0; i < max; i++)
            {
                var vehicles = TryReverseRides(i);
                var score = vehicles.Sum(v => v.TotalScore);

                var bestScore = bestVehicles.Sum(v => v.TotalScore);
                if (score > bestScore)
                    bestVehicles = vehicles;
            }

            return bestVehicles;
        }

        public override string ToString()
        {
            return $"Rows: {Rows:n0}, Columns: {Columns:n0}, Vehicles: {Vehicles:n0}, Rides: {Rides:n0}, Bonus: {Bonus:n0}, Steps: {Steps:n0}";
        }
    }
}
