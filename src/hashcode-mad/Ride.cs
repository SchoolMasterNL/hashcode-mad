namespace hashcode_mad
{
    internal class Ride
    {
        public int Id { get; set; }

        public int StartX { get; set; }

        public int StartY { get; set; }

        public int EndX { get; set; }

        public int EndY { get; set; }

        public int Start { get; set; }

        public int End { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, StartX: {StartX}, StartY: {StartY}, EndX: {EndX}, EndY: {EndY}, Start: {Start}, End: {End}";
        }
    }
}