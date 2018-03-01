﻿namespace hashcode_mad
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

        public override string ToString()
        {
            return $"Rows: {Rows}, Columns: {Columns}, Vehicles: {Vehicles}, Rides: {Rides}, Bonus: {Bonus}, Steps: {Steps}";
        }
    }
}