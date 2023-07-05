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

        [ReadOnly]
        public NativeParallelMultiHashMap<int, AvoidanceAgentData> agentsPerCell;

        public void Execute(FlowFieldAgentAspect agentAspect)
        {
            agentAspect.AvoidanceCounter = 0;
            var cellKey = AgentAvoidanceSystem.GetHashForPosition(agentAspect.Position.xz, cellSize);
            if (!agentsPerCell.TryGetFirstValue(cellKey, out var neighborData, out var iterator))
            {
                return;
            }

            var myPosition = agentAspect.Position;
            var closestDistance = float.MaxValue;
            var closestPosition = float3.zero;
            do
            {
                if (agentAspect.Self == neighborData.AgentEntity)
                {
                    continue;
                }

                if (UpdateClosestDistance(
                        (agentAspect.AvoidanceRadius + neighborData.AgentRadius).Squared(),
                        ref closestDistance,
                        ref closestPosition,
                        myPosition,
                        neighborData.Position,
                        neighborData.AgentRadius))
                {
                    agentAspect.AvoidanceCounter++;
                }
            }
            while (agentsPerCell.TryGetNextValue(out neighborData, ref iterator));

            agentAspect.AvoidanceVector = agentAspect.AvoidanceCounter > 0
                ? math.normalize(myPosition - closestPosition)
                : float3.zero;
        }

        private static bool UpdateClosestDistance(
            in float avoidanceRadiusSqrt,
            ref float closestDistance,
            ref float3 closetsPosition,
            in float3 myPosition,
            in float3 neighborPosition,
            in float neighborRadius)
        {
            var distanceToNeighbor = math.distancesq(neighborPosition, myPosition);
            if (distanceToNeighbor > avoidanceRadiusSqrt)
            {
                return false;
            }

            if (distanceToNeighbor > closestDistance)
            {
                return false;
            }

            closestDistance = distanceToNeighbor;
            closetsPosition = neighborPosition;
            return true;
        }
    }
}