using IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Aspects;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Systems
{
    [BurstCompile]
    public partial struct CalculateClosestAvoidanceVectorJob : IJobEntity
    {
        public int cellSize;
        [ReadOnly] public NativeParallelMultiHashMap<int, float3> agentsPerCell;

        public void Execute(FlowFieldAgentAspect agentAspect)
        {
            agentAspect.AvoidanceCounter = 0;
            var cellKey = AgentAvoidanceSystem.GetHashForPosition(agentAspect.Position.xz, cellSize);
            if (!agentsPerCell.TryGetFirstValue(cellKey, out var neighborPosition, out var iterator)) 
                return;
            
            var myPosition = agentAspect.Position;
            var closestDistance = float.MaxValue;
            var closestPosition = float3.zero;
            do
            {
                UpdateClosestDistance(
                    math.pow(agentAspect.AvoidanceRadius, 2),
                    ref closestDistance, 
                    ref closestPosition, 
                    myPosition,
                    neighborPosition);
            }
            while (agentsPerCell.TryGetNextValue(out neighborPosition, ref iterator));
            
            agentAspect.AvoidanceCounter = 1;
            agentAspect.AvoidanceVector = myPosition - closestPosition;
        }

        private static void UpdateClosestDistance(
            in float avoidanceRadiusSqrt, 
            ref float closestDistance,
            ref float3 closetsPosition,
            in float3 myPosition,
            in float3 neighborPosition)
        {
            var distanceToNeighbor = math.distancesq(neighborPosition, myPosition);
            if(distanceToNeighbor <= float.Epsilon)
                return;

            if(distanceToNeighbor > avoidanceRadiusSqrt)
                return;
            
            if (distanceToNeighbor > closestDistance) 
                return;
            
            closestDistance = distanceToNeighbor;
            closetsPosition = neighborPosition;
        }
    }
}