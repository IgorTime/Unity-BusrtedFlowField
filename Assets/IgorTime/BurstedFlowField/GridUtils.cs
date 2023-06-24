using Unity.Mathematics;
using UnityEngine;

namespace IgorTime.BurstedFlowField
{
    public static class GridUtils
    {
        public static int GetCellIndex(in Vector2Int gridSize, in Vector2Int cellCoordinates)
        {
            return GetCellIndex(gridSize, cellCoordinates.x, cellCoordinates.y);
        }

        public static int GetCellIndex(in Vector2Int gridSize, in int x, in int y)
        {
            return x + y * gridSize.x;
        }

        public static float2 GetCellPosition(
            in int cellX,
            in int cellY,
            in float cellRadius)
        {
            var cellDiameter = cellRadius * 2;
            return new float2
            {
                x = cellX * cellDiameter + cellRadius,
                y = cellY * cellDiameter + cellRadius
            };
        }
    }
}