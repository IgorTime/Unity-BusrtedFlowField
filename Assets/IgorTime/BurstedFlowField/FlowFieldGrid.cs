using System;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField
{
    [Serializable]
    public struct FlowFieldGrid
    {
        public int cellsCount;
        public int2 gridSize;
        public float cellRadius;
        public float2[] cellPositions;
        public byte[] costField;
        public ushort[] integrationField;

        public static FlowFieldGrid CreateGrid(int2 gridSize, float cellRadius)
        {
            var cellsCount = gridSize.x * gridSize.y;
            var grid = new FlowFieldGrid
            {
                cellsCount = cellsCount,
                gridSize = gridSize,
                cellRadius = cellRadius,
                cellPositions = GridUtils.GetCellPositions(gridSize, cellRadius),
                costField = new byte[cellsCount],
                integrationField = new ushort[cellsCount]
            };

            return grid;
        }
    }
}