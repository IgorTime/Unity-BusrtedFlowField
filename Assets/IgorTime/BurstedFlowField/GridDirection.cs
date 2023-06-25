using Unity.Mathematics;

namespace IgorTime.BurstedFlowField
{
    public static class GridDirection
    {
        public static readonly int2 North = new(0, 1);
        public static readonly int2 East = new(1, 0);
        public static readonly int2 West = new(-1, 0);
        public static readonly int2 South = new(0, -1);
        public static readonly int2 NorthEast = new(1, 1);
        public static readonly int2 NorthWest = new(-1, 1);
        public static readonly int2 SouthEast = new(1, -1);
        public static readonly int2 SouthWest = new(-1, -1);

        public static readonly byte NorthPacked = 0;
        public static readonly byte EastPacked = 1;
        public static readonly byte WestPacked = 2;
        public static readonly byte SouthPacked = 3;
        public static readonly byte NorthEastPacked = 4;
        public static readonly byte NorthWestPacked = 5;
        public static readonly byte SouthEastPacked = 6;
        public static readonly byte SouthWestPacked = 7;

        public static readonly int2[] AllDirections =
        {
            North,
            East,
            West,
            South,
            NorthEast,
            NorthWest,
            SouthEast,
            SouthWest
        };

        public static readonly int2[] CardinalDirections =
        {
            North,
            East,
            West,
            South
        };

        public static readonly int2[] DiagonalDirections =
        {
            NorthEast,
            NorthWest,
            SouthEast,
            SouthWest
        };

        public static int2 Unpack(in byte packedDirection)
        {
            return AllDirections[packedDirection];
        }

        public static byte PackDirection(in int2 direction)
        {
            if (direction.Equals(North)) return NorthPacked;
            if (direction.Equals(East)) return EastPacked;
            if (direction.Equals(West)) return WestPacked;
            if (direction.Equals(South)) return SouthPacked;
            if (direction.Equals(NorthEast)) return NorthEastPacked;
            if (direction.Equals(NorthWest)) return NorthWestPacked;
            if (direction.Equals(SouthEast)) return SouthEastPacked;
            if (direction.Equals(SouthWest)) return SouthWestPacked;

            return 0;
        }
    }
}