using System;
using System.Collections.Generic;
using System.Linq;

namespace hashcode_mad
{
    internal class ReverseVehicle
    {
        private readonly int id;
        private readonly int bonus;
        private readonly List<Ride> rides;

        public ReverseVehicle(int id, int bonus)
        {
            this.id = id;
            this.bonus = bonus;

            rides = new List<Ride>();
        }

        public int CurrentStep { get; private set; }

        public Vehicle ToVehicle()
        {
            var result = new Vehicle(id, bonus);

            var reverse = rides.AsEnumerable().Reverse().ToList();
            foreach (var ride in reverse)
            {
                result.AssignRide2(ride);
            }

            return result;
        }

        public int EarliestStart(Ride ride)
        {
            var extra = Distance(ride);

            return Math.Min(CurrentStep - extra - ride.Distance, ride.End - 1 - ride.Distance);
        }

        public bool CanStart(Ride ride)
        {
            var earliestStart = EarliestStart(ride);

            if (earliestStart <= ride.Start)
                return false;

            if (earliestStart < 0)
                return false;

            var fromStart = Manhattan.Distance(ride.StartX, ride.StartY, 0, 0);

            return earliestStart - fromStart > 0;
        }

        public void SetEnd(Ride ride)
        {
            this.rides.Add(ride);

            this.CurrentStep = ride.End - ride.Distance;
        }

        public void AssignRide(Ride ride)
        {
            CurrentStep = EarliestStart(ride);

            rides.Add(ride);
        }

        public int Distance(Ride ride)
        {
            var lastRide = rides.LastOrDefault();

            return Manhattan.Distance(lastRide.StartX, lastRide.StartY, ride.EndX, ride.EndY);
        }
    }
}