﻿using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField
{
    public static class GridExtensions
    {
        public static int GetCellIndex(in this FlowFieldGrid grid, in int2 cellCoordinates)
        {
            return GridUtils.GetCellIndex(grid.gridSize, cellCoordinates.x, cellCoordinates.y);
        }

        public static void GetCardinalNeighbors(
            in this FlowFieldGrid grid,
            in int cellIndex,
            ref UnsafeList<int> neighbors)
        {
            GridUtils.GetNeighbors(
                grid.gridSize,
                cellIndex,
                GridDirection.CardinalDirections,
                ref neighbors);
        }

        public static void GetAllNeighbors(
            in this FlowFieldGrid grid,
            in int cellIndex,
            ref UnsafeList<int> neighbors)
        {
            GridUtils.GetNeighbors(
                grid.gridSize,
                cellIndex,
                GridDirection.AllDirections,
                ref neighbors);
        }

        public static int2 GetFlowDirection(
            in this FlowFieldGrid grid,
            in int cellIndex)
        {
            return GridDirection.Unpack(grid.vectorField[cellIndex]);
        }
    }
}