﻿using System;

namespace hashcode_mad
{
    internal class Ride
    {
        public Ride(int id, int startX, int startY, int endX, int endY, int start, int end)
        {
            this.Id = id;
            this.StartX = startX;
            this.StartY = startY;
            this.EndX = endX;
            this.EndY = endY;
            this.Start = start;
            this.End = end;

            this.Distance = Manhattan.Distance(this.StartX, this.StartY, this.EndX, this.EndY);
        }

        public int Id { get; }

        public int StartX { get; }

        public int StartY { get; }

        public int EndX { get; }

        public int EndY { get; }

        public int Start { get; }

        public int End { get; }

        public int Distance { get; }

        public int GetDistance(int x, int y)
        {
            return Manhattan.Distance(StartX, StartY, x, y);
        }

        public override string ToString()
        {
            return $"Id: {Id}, StartX: {StartX}, StartY: {StartY}, EndX: {EndX}, EndY: {EndY}, Start: {Start}, End: {End}, Distance: {Distance}";
        }
    }
}