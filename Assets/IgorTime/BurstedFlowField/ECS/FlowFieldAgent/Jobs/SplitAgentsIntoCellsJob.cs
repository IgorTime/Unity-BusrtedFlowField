using IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Aspects;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Systems
{
    [BurstCompile]
    public partial struct SplitAgentsIntoCellsJob : IJobEntity
    {
        public int cellSize;
        
        [WriteOnly]
        public NativeParallelMultiHashMap<int, AvoidanceAgentData>.ParallelWriter hashMapWriter;

        // public void Execute(FlowFieldAgentAspect agentAspect)
        // {
        //     var position = agentAspect.Position;
        //     var corner1 = position.xz - agentAspect.AvoidanceRadius;
        //     var corner2 = position.xz + agentAspect.AvoidanceRadius;
        //     var corner3 = new float2(corner1.x, corner2.y);
        //     var corner4 = new float2(corner2.x, corner1.y);
        //
        //     var hashSet = new UnsafeHashSet<int>(4, Allocator.Temp);
        //     hashSet.Add(AgentAvoidanceSystem.GetHashForPosition(corner1, cellSize));
        //     hashSet.Add(AgentAvoidanceSystem.GetHashForPosition(corner2, cellSize));
        //     hashSet.Add(AgentAvoidanceSystem.GetHashForPosition(corner3, cellSize));
        //     hashSet.Add(AgentAvoidanceSystem.GetHashForPosition(corner4, cellSize));
        //
        //     var finalColor = Color.white;
        //     foreach (var cellHash in hashSet)
        //     {
        //         hashMapWriter.Add(cellHash, position);
        //
        //         finalColor = cellHash % 2 == 0 ? Color.red : Color.blue;
        //     }
        //
        //     EcsGizmosDrawer.DrawCube(
        //         agentAspect.Position, 
        //         Vector3.one * agentAspect.Radius * 0.5f, 
        //         finalColor
        //         );
        // }
        
        public void Execute(FlowFieldAgentAspect agentAspect)
        {
            var position = agentAspect.Position;
            var hash = AgentAvoidanceSystem.GetHashForPosition(position.xz, cellSize);
            
            hashMapWriter.Add(hash, new AvoidanceAgentData(
                agentAspect.Self,
                position,
                agentAspect.Radius));
        }
    }
}