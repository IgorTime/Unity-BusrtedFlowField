using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

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
            var grid = new FlowFieldGrid
            {
                cellsCount = gridSize.x * gridSize.y,
                gridSize = gridSize,
                cellRadius = cellRadius,
                cellPositions = GridUtils.GetCellPositions(gridSize, cellRadius)
            };

            return grid;
        }
    }
}