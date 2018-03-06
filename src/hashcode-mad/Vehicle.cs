using System;
using System.Collections.Generic;
using System.Linq;

namespace hashcode_mad
{
    internal class Vehicle
    {
        private List<Ride> rides;
        private int addedSteps;
        private int bonus;
        private Dictionary<int, int> distanceCache;
        private Dictionary<int, int> scoreCache;

        public Vehicle(int id, int bonus)
        {
            Id = id;
            this.bonus = bonus;
            rides = new List<Ride>();

            addedSteps = 0;
            Score = 0;
            Bonus = 0;

            distanceCache = new Dictionary<int, int>();
            scoreCache = new Dictionary<int, int>();
        }

        public IEnumerable<Ride> Rides => rides;

        public int Id { get; }

        public int CurrentStep { get; private set; }

        public int Score { get; private set; }

        public int Bonus { get; private set; }

        public void AssignRide(Ride ride)
        {
            (var score, var bonus, var extraSteps) = Calculate(ride);

            Score += score;
            Bonus += bonus;
            addedSteps += extraSteps;

            rides.Add(ride);

            CurrentStep = rides.Sum(r => r.Distance) + addedSteps;

            scoreCache.Clear();
            distanceCache.Clear();
        }

        public override string ToString()
        {
            (int x, int y) = Position();
            return $"Id: {Id}, PosX: {x}, PosY: {y}, CurrentStep: {CurrentStep}, Score: {Score}, Bonus {Bonus}, Total: {Score + Bonus}";
        }

        public int GetDistance(Ride ride)
        {
            if (!distanceCache.ContainsKey(ride.Id))
            {
                (int x, int y) = Position();

                distanceCache[ride.Id] = Math.Abs(x - ride.StartX) + Math.Abs(y - ride.StartY);
            }

            return distanceCache[ride.Id];
        }

        public int GetScoreAndBonus(Ride ride)
        {
            (var score, var bonus, var extraSteps) = Calculate(ride);

            return score + bonus;
        }

        public bool CanFinish(Ride ride)
        {
            return CurrentStep + GetDistance(ride) + ride.Distance < ride.End;
        }

        public int GetRideScore(Ride ride)
        {
            var earliestStart = CurrentStep + GetDistance(ride);
            var score = ride.Start - earliestStart;

            if (score == 0)
                return bonus;

            if (score < 0)
                return bonus - score;

            return score;
        }

        public int GetRideScore2(Ride ride)
        {
            if (!scoreCache.ContainsKey(ride.Id))
            {
                var earliestStart = CurrentStep + GetDistance(ride);
                var score = ride.Start - earliestStart;
                var extra = ride.Distance;

                if (score == 0) // exact on time
                    score = bonus + extra;
                else if (score >= 0) // earlier
                    score = (bonus - score) + extra;
                else // late
                    score = score + extra;

                scoreCache[ride.Id] = score;
            }

            return scoreCache[ride.Id];
        }

        public int GetRideScore3(Ride ride)
        {
            if (!scoreCache.ContainsKey(ride.Id))
            {
                var earliestStart = CurrentStep + GetDistance(ride);
                var score = ride.Start - earliestStart;
                var extra = 0;

                if (score == 0) // exact on time
                    score = bonus + extra;
                else if (score >= 0) // earlier
                    score = (bonus - score) + extra;
                else // late
                    score = score + extra;

                scoreCache[ride.Id] = score;
            }

            return scoreCache[ride.Id];
        }

        private (int x, int y) Position()
        {
            if (rides.Count == 0)
            {
                return (0, 0);
            }

            var lastRide = rides[rides.Count - 1];
            return (lastRide.EndX, lastRide.EndY);
        }

        private (int score, int bonus, int extraSteps) Calculate(Ride ride)
        {
            int score = ride.Distance;
            int extraSteps = GetDistance(ride);
            int bonus = 0;

            int start = CurrentStep + extraSteps;
            if (start <= ride.Start)
            {
                bonus = this.bonus;
                extraSteps += ride.Start - start;
            }

            return (score, bonus, extraSteps);
        }
    }
}
