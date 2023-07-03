using Unity.Mathematics;

namespace IgorTime.BurstedFlowField
{
    public static class GridDirection
    {
        public static readonly int2 None = new(0, 0);
        public static readonly int2 North = new(0, 1);
        public static readonly int2 East = new(1, 0);
        public static readonly int2 West = new(-1, 0);
        public static readonly int2 South = new(0, -1);
        public static readonly int2 NorthEast = new(1, 1);
        public static readonly int2 NorthWest = new(-1, 1);
        public static readonly int2 SouthEast = new(1, -1);
        public static readonly int2 SouthWest = new(-1, -1);

        private static readonly byte NonePacked = 0;
        private static readonly byte NorthPacked = 1;
        private static readonly byte EastPacked = 2;
        private static readonly byte WestPacked = 3;
        private static readonly byte SouthPacked = 4;
        private static readonly byte NorthEastPacked = 5;
        private static readonly byte NorthWestPacked = 6;
        private static readonly byte SouthEastPacked = 7;
        private static readonly byte SouthWestPacked = 8;

        public static readonly int2[] AllDirections =
        {
            None,
            North,
            East,
            West,
            South,
            NorthEast,
            NorthWest,
            SouthEast,
            SouthWest
        };

        private static readonly float2 NoneMoveDirection = float2.zero;
        private static readonly float2 NorthMoveDirection = math.normalize(North);
        private static readonly float2 EastMoveDirection = math.normalize(East);
        private static readonly float2 WestMoveDirection = math.normalize(West);
        private static readonly float2 SouthMoveDirection = math.normalize(South);
        private static readonly float2 NorthEastMoveDirection = math.normalize(NorthEast);
        private static readonly float2 NorthWestMoveDirection = math.normalize(NorthWest);
        private static readonly float2 SouthEastMoveDirection = math.normalize(SouthEast);
        private static readonly float2 SouthWestMoveDirection = math.normalize(SouthWest);

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
            switch (packedDirection)
            {
                case 0: return None;
                case 1: return North;
                case 2: return East;
                case 3: return West;
                case 4: return South;
                case 5: return North;
                case 6: return NorthWest;
                case 7: return SouthEast;
                case 8: return SouthWest;
                default: return None;
            }
        }

        public static float2 UnpackAsMoveDirection(in byte packedDirection)
        {
            switch (packedDirection)
            {
                case 0: return NoneMoveDirection;
                case 1: return NorthMoveDirection;
                case 2: return EastMoveDirection;
                case 3: return WestMoveDirection;
                case 4: return SouthMoveDirection;
                case 5: return NorthEastMoveDirection;
                case 6: return NorthWestMoveDirection;
                case 7: return SouthEastMoveDirection;
                case 8: return SouthWestMoveDirection;
                default: return NoneMoveDirection;
            }
        }

        public static byte PackDirection(in int2 direction)
        {
            if (direction.Equals(None)) return NonePacked;
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