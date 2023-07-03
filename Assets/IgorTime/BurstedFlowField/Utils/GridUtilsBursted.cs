using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField
{
    [BurstCompile]
    public static class GridUtilsBursted
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 GetWorldPositionFromCell(
            in float cellRadius,
            in int2 cellCoordinates)
        {
            var cellDiameter = cellRadius * 2;
            var startPosition = cellRadius;
            return new float3
            {
                x = startPosition + cellCoordinates.x * cellDiameter,
                y = 0,
                z = startPosition + cellCoordinates.y * cellDiameter,
            };
        }
    }
}