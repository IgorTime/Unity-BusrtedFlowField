using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField.ECS.Data
{
    public struct AvoidanceGrid : IComponentData
    {
        public NativeParallelMultiHashMap<int, float3> agentsPerCell;
    }
}