using System;
using System.Collections.Generic;
using System.Text;

namespace hashcode_mad
{
    internal class Vehicle
    {
        private List<Ride> rides;

        public Vehicle(int id)
        {
            Id = id;
            rides = new List<Ride>();
        }

        public IEnumerable<Ride> Rides => rides;

        public int Id { get; }

        public void AssignRide(Ride ride)
        {
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
