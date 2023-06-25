using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField.ECS.Data
{
    public struct FlowFieldData : IComponentData
    {
        public int cellsCount;
        public float cellRadius;
        public int2 gridSize;
    }
}