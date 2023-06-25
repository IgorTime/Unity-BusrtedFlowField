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
        
        public static float2[] GetCellPositions(Vector2Int gridSize, float cellRadius)
        {
            var result = new float2[gridSize.x * gridSize.y];

            for (var y = 0; y < gridSize.y; y++)
            for (var x = 0; x < gridSize.x; x++)
            {
                var cellIndex = GetCellIndex(gridSize, x, y);
                result[cellIndex] = GetCellPosition(x, y, cellRadius);
            }

            return result;
        }
    }
}