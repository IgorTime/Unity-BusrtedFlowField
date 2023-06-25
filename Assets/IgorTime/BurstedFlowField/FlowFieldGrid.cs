using System;
using Unity.Collections;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField
{
    [Serializable]
    public struct FlowFieldGrid : IDisposable
    {
        public int cellsCount;
        public int2 gridSize;
        public float cellRadius;
        public float2[] cellPositions;
        public byte[] costField;
        public NativeArray<ushort> integrationField;
        public NativeArray<byte> vectorField;

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
                integrationField = new NativeArray<ushort>(cellsCount, Allocator.Persistent),
                vectorField = new NativeArray<byte>(cellsCount, Allocator.Persistent),
            };

            return grid;
        }

        public void Dispose()
        {
            integrationField.Dispose();
            vectorField.Dispose();
        }
    }
}