using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hashcode_mad
{
    internal class Vehicle
    {
        private List<Ride> rides;
        private int addedSteps;
        private int bonus;

        public Vehicle(int id, int bonus)
        {
            Id = id;
            rides = new List<Ride>();

            addedSteps = 0;
            this.bonus = bonus;
            Score = 0;
        }

        public IEnumerable<Ride> Rides => rides;

        public int Id { get; }

        public int CurrentStep => rides.Sum(r => r.Distance) + addedSteps;

        public int Score { get; set; }

        public void AssignRide(Ride ride)
        {
            this.addedSteps += GetDistance(ride);

            if (CurrentStep  <= ride.Start)
            {
                this.addedSteps += ride.Start - CurrentStep;

                Score += bonus;
            }

            Score += ride.Distance;

            rides.Add(ride);
        }

        public int GetDistance(Ride ride)
        {
            int x = 0;
            int y = 0;

            if (rides.Count > 0)
            {
                var lastRide = rides[rides.Count - 1];
                x = lastRide.EndX;
                y = lastRide.EndY;
            }

            return Math.Abs(x + ride.StartX) + Math.Abs(y + ride.StartY);
        }
    }
}
