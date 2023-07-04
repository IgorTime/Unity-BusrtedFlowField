using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField.ECS.Data
{
    public struct AvoidanceGrid : IComponentData
    {
        public const int X_SIZE = 1000;
        public const int Y_SIZE = 1000;
        
        public NativeParallelMultiHashMap<int, float3> agentsPerCell;
    }
}