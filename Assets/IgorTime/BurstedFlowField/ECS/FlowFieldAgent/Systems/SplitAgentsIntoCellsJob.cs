using IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Aspects;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Systems
{
    [BurstCompile]
    public partial struct SplitAgentsIntoCellsJob : IJobEntity
    {
        public int cellSize;
        // [WriteOnly] public NativeParallelMultiHashMap<int, FlowFieldAgentAspect> agentsPerCell;
        [WriteOnly] public NativeParallelMultiHashMap<int, float3>.ParallelWriter hashMapWriter;

        public void Execute(FlowFieldAgentAspect agentAspect)
        {
            var position = agentAspect.Position;
            var corner1 = position.xz - agentAspect.AvoidanceRadius;
            var corner2 = position.xz + agentAspect.AvoidanceRadius;
            var corner3 = new float2(corner1.x, corner2.y);
            var corner4 = new float2(corner2.x, corner1.y);
            
            var hashSet = new UnsafeHashSet<int>(4, Allocator.Temp);
            hashSet.Add(AgentAvoidanceSystem.GetHashForPosition(corner1, cellSize));
            hashSet.Add(AgentAvoidanceSystem.GetHashForPosition(corner2, cellSize));
            hashSet.Add(AgentAvoidanceSystem.GetHashForPosition(corner3, cellSize));
            hashSet.Add(AgentAvoidanceSystem.GetHashForPosition(corner4, cellSize));

            foreach (var cellHash in hashSet)
            {
                hashMapWriter.Add(cellHash, position);
            }
        }
    }
}