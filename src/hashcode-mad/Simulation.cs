﻿using System;
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
            };

            var bestVehicles = (IEnumerable<Vehicle>)null;
            var bestTotal = 0;

            for (int i = 0; i < runs.Length; i++)
            {
                var vehicles = runs[i](input);

                var score = vehicles.Sum(v => v.Score);
                var bonus = vehicles.Sum(v => v.Bonus);
                var total = score + bonus;

                Console.WriteLine($"Run{i + 1}= Score: {score}, Bonus {bonus}, Total: {total}");
                if (total > bestTotal)
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

                var bestVehicle = vehicles.OrderBy(_ => _.RideScore(ride)).First();
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
                var assigned = false;
                foreach (var vehicle in result)
                {
                    var vehicleRides = rides.Where(_ => vehicle.CanFinish(_));
                    if (vehicleRides.Count() == 0)
                        continue;

                    var bestRide = vehicleRides.OrderBy(_ => vehicle.RideScore(_)).First();
                    vehicle.AssignRide(bestRide);
                    rides.Remove(bestRide);
                    assigned = true;
                }

                if (!assigned)
                    break;
            }

            return result;
        }


        public override string ToString()
        {
            return $"Rows: {Rows}, Columns: {Columns}, Vehicles: {Vehicles}, Rides: {Rides}, Bonus: {Bonus}, Steps: {Steps}";
        }
    }
}
