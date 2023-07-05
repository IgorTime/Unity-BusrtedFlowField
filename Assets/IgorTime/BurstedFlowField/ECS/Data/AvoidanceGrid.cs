using IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Systems;
using Unity.Collections;
using Unity.Entities;

namespace IgorTime.BurstedFlowField.ECS.Data
{
    public struct AvoidanceGrid : IComponentData
    {
        public const int X_SIZE = 1000;
        public const int Y_SIZE = 1000;

        public NativeParallelMultiHashMap<int, AvoidanceAgentData> agentsPerCell;
    }
}