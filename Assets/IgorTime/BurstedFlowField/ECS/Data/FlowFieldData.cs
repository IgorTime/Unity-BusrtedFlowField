using Unity.Entities;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField.ECS.Data
{
    public readonly struct FlowFieldData : IComponentData
    {
        public readonly int cellsCount;
        public readonly float cellRadius;
        public readonly int2 gridSize;
        public readonly float with;
        public readonly float height;

        public FlowFieldData(float cellRadius, int2 gridSize)
        {
            cellsCount = gridSize.x * gridSize.y;
            with = gridSize.x * cellRadius * 2;
            height = gridSize.y * cellRadius * 2;
            this.gridSize = gridSize;
            this.cellRadius = cellRadius;
        }
    }
}