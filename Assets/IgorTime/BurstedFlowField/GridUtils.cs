using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField
{
    [BurstCompile]
    public static class GridUtils
    {
        public static int GetCellIndex(in int2 gridSize, in int2 cellCoordinates)
        {
            return GetCellIndex(gridSize, cellCoordinates.x, cellCoordinates.y);
        }

        public static int GetCellIndex(in int2 gridSize, in int x, in int y)
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

        public static float2[] GetCellPositions(in int2 gridSize, in float cellRadius)
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

        public static void GetNeighbors(
            in int2 gridSize,
            in int cellIndex,
            int2[] directions,
            ref UnsafeList<int> result)
        {
            result.Clear();
            var cellCoordinates = GetCellCoordinates(gridSize, cellIndex);
            for (var i = 0; i < 4; i++)
            {
                var neighbor = cellCoordinates + directions[i];
                if (IsCellInGrid(gridSize, neighbor)) result.Add(GetCellIndex(gridSize, neighbor));
            }
        }
        
        public static void GetNeighbors(
            in int2 gridSize,
            in int cellIndex,
            in NativeArray<int2> directions,
            ref UnsafeList<int> result)
        {
            result.Clear();
            var cellCoordinates = GetCellCoordinates(gridSize, cellIndex);
            for (var i = 0; i < 4; i++)
            {
                AppendNeighbor(gridSize, cellCoordinates, directions[i], ref result);
            }
        }
        
        public static void GetAllNeighbors(
            in int2 gridSize,
            in int cellIndex,
            ref UnsafeList<int> result)
        {
            result.Clear();
            var cellCoordinates = GetCellCoordinates(gridSize, cellIndex);
            AppendNeighbor(gridSize, cellCoordinates, GridDirection.North, ref result);
            AppendNeighbor(gridSize, cellCoordinates, GridDirection.East, ref result);
            AppendNeighbor(gridSize, cellCoordinates, GridDirection.South, ref result);
            AppendNeighbor(gridSize, cellCoordinates, GridDirection.West, ref result);
            AppendNeighbor(gridSize, cellCoordinates, GridDirection.NorthEast, ref result);
            AppendNeighbor(gridSize, cellCoordinates, GridDirection.NorthWest, ref result);
            AppendNeighbor(gridSize, cellCoordinates, GridDirection.SouthEast, ref result);
            AppendNeighbor(gridSize, cellCoordinates, GridDirection.SouthWest, ref result);
        }
        
        public static void GetCardinalNeighbors(
            in int2 gridSize,
            in int cellIndex,
            ref UnsafeList<int> result)
        {
            result.Clear();
            var cellCoordinates = GetCellCoordinates(gridSize, cellIndex);
            AppendNeighbor(gridSize, cellCoordinates, GridDirection.North, ref result);
            AppendNeighbor(gridSize, cellCoordinates, GridDirection.East, ref result);
            AppendNeighbor(gridSize, cellCoordinates, GridDirection.South, ref result);
            AppendNeighbor(gridSize, cellCoordinates, GridDirection.West, ref result);
        }
        
        public static void AppendNeighbor(
            in int2 gridSize, 
            in int2 cellCoordinates, 
            in int2 direction, 
            ref UnsafeList<int> result)
        {
            var neighbor = cellCoordinates + direction;
            if (IsCellInGrid(gridSize, neighbor)) 
                result.Add(GetCellIndex(gridSize, neighbor));
        }

        public static int2 GetCellCoordinates(in int2 gridSize, int cellIndex)
        {
            var cellsCount = gridSize.x * gridSize.y;
            if (cellIndex < 0 || cellIndex >= cellsCount)
                throw new ArgumentOutOfRangeException(nameof(cellIndex), cellIndex,
                    $"Cell index must be in range [0, {cellsCount})");

            return new int2
            {
                x = cellIndex % gridSize.x,
                y = cellIndex / gridSize.x
            };
        }

        public static bool IsCellInGrid(in int2 gridSize, in int2 cellCoordinates)
        {
            return math.all(cellCoordinates >= 0) && math.all(cellCoordinates < gridSize);
        }
    }
}