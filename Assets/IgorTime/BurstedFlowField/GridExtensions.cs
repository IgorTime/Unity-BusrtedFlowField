using UnityEngine;

namespace IgorTime.BurstedFlowField
{
    public static class GridExtensions
    {
        public static int GetCellIndex(in this FlowFieldGrid grid, in Vector2Int cellCoordinates)
        {
            return GridUtils.GetCellIndex(grid.gridSize, cellCoordinates.x, cellCoordinates.y);
        }
    }
}