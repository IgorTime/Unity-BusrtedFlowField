using IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Aspects;
using IgorTime.BurstedFlowField.ECS.Systems;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(FlowFieldSystemGroup))]
    [UpdateAfter(typeof(CalculateFlowFieldSystem))]
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
            {
                return;
            }

            agentsPerCell.Clear();
            if (entitiesCount > agentsPerCell.Capacity)
            {
                agentsPerCell.Capacity = entitiesCount * 4;
            }

            const int AVOIDANCE_GRID_CELL_SIZE = 2;
            new SplitAgentsIntoCellsJob
            {
                cellSize = AVOIDANCE_GRID_CELL_SIZE,
                hashMapWriter = agentsPerCell.AsParallelWriter(),
            }.ScheduleParallel();

            new CalculateClosestAvoidanceVectorJob
            {
                cellSize = AVOIDANCE_GRID_CELL_SIZE,
                agentsPerCell = agentsPerCell,
            }.ScheduleParallel();
        }

        public static int GetHashForPosition(in float3 position, in int cellSize) =>
            (int) (19 * math.floor(position.x / cellSize) +
                   17 * math.floor(position.z / cellSize));

        public static int GetHashForPosition(in float2 position, in int cellSize) =>
            (int) (19 * math.floor(position.x / cellSize) +
                   17 * math.floor(position.y / cellSize));
    }
}