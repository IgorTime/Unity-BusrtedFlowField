using IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Aspects;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Systems
{
    [BurstCompile]
    public partial struct AgentAvoidanceSystem : ISystem
    {
        private NativeParallelMultiHashMap<int, FlowFieldAgentAspect> agentsPerCell;
        private EntityQuery agentsQuery;

        public void OnCreate(ref SystemState state)
        {
            agentsPerCell = new NativeParallelMultiHashMap<int, FlowFieldAgentAspect>(100, Allocator.Persistent);

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

            var avoidanceGridCellSize = 5;
            var j1 = new SplitAgentsIntoCellsJob
            {
                cellSize = avoidanceGridCellSize,
                hashMapWriter = agentsPerCell.AsParallelWriter()
            };

            j1.ScheduleParallel();

            // var j2 = new CalculateAvoidanceVectorJob
            // {
            //     cellSize = 5,
            //     agentsPerCell = agentsPerCell
            // };

            // agentsPerCell.Dispose();
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
    public partial struct CalculateAvoidanceVectorJob : IJobEntity
    {
        public int cellSize;
        [ReadOnly] public UnsafeParallelMultiHashMap<int, FlowFieldAgentAspect> agentsPerCell;

        public void Execute(FlowFieldAgentAspect agentAspect)
        {
            var cellKey = AgentAvoidanceSystem.GetHashForPosition(agentAspect.Position.xz, cellSize);
        }
    }
}