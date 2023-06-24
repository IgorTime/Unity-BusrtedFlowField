using System;
using Unity.Mathematics;
using UnityEngine;

namespace IgorTime.BurstedFlowField
{
    [Serializable]
    public struct FlowFieldGrid
    {
        public int cellsCount;
        public Vector2Int gridSize;
        public float cellRadius;
        public float2[] cellPositions;
        public byte[] costField;

        public static FlowFieldGrid CreateGrid(Vector2Int gridSize, float cellRadius)
        {
            var grid = new FlowFieldGrid
            {
                cellsCount = gridSize.x * gridSize.y,
                gridSize = gridSize,
                cellRadius = cellRadius,
                cellPositions = GetCellPositions(gridSize, cellRadius)
            };

            return grid;
        }

        private static float2[] GetCellPositions(Vector2Int gridSize, float cellRadius)
        {
            var result = new float2[gridSize.x * gridSize.y];

            for (var y = 0; y < gridSize.y; y++)
            for (var x = 0; x < gridSize.x; x++)
            {
                var cellIndex = GridUtils.GetCellIndex(gridSize, x, y);
                result[cellIndex] = GridUtils.GetCellPosition(x, y, cellRadius);
            }

            return result;
        }
    }
}