using IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Aspects;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Systems
{
    [BurstCompile]
    public partial struct AgentAvoidanceSystem : ISystem
    {
        private NativeParallelMultiHashMap<int, float3> agentsPerCell;
        private EntityQuery agentsQuery;

        public void OnCreate(ref SystemState state)
        {
            agentsPerCell = new NativeParallelMultiHashMap<int, float3>(100, Allocator.Persistent);

            agentsQuery = SystemAPI.QueryBuilder()
                .WithAspect<FlowFieldAgentAspect>()
                .Build();
        }

        public void OnUpdate(ref SystemState state)
        {
            var entitiesCount = agentsQuery.CalculateEntityCount();
            if (entitiesCount == 0)
                return;

            agentsPerCell.Clear();
            if (entitiesCount > agentsPerCell.Capacity)
                agentsPerCell.Capacity = entitiesCount * 2;

            const int avoidanceGridCellSize = 5;
            new SplitAgentsIntoCellsJob
            {
                cellSize = avoidanceGridCellSize,
                hashMapWriter = agentsPerCell.AsParallelWriter()
            }.ScheduleParallel();

            new CalculateClosestAvoidanceVectorJob
            {
                cellSize = 5,
                agentsPerCell = agentsPerCell
            }.ScheduleParallel();
        }

        public static int GetHashForPosition(in float3 position, in int cellSize)
        {
            return (int)(19 * math.floor(position.x / cellSize) +
                         17 * math.floor(position.z / cellSize));
        }

        public static int GetHashForPosition(in float2 position, in int cellSize)
        {
            return (int)(19 * math.floor(position.x / cellSize) +
                         17 * math.floor(position.y / cellSize));
        }
    }

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