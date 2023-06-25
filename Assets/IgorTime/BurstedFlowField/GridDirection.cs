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
    }
}