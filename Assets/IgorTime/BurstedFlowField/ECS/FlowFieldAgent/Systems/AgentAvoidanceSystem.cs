using IgorTime.BurstedFlowField.ECS.Data;
using IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Aspects;
using IgorTime.BurstedFlowField.ECS.Systems;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(FlowFieldSystemGroup))]
    [UpdateAfter(typeof(CalculateFlowFieldSystem))]
    public partial struct AgentAvoidanceSystem : ISystem
    {
        public const float ARRIVAL_DISTANCE = 0.2f;
        
        private EntityQuery agentsQuery;
        
        public JobHandle SplitAgentsHandle { get; private set; }

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<AvoidanceGrid>();
            var singleton = state.EntityManager.CreateSingleton<AvoidanceGrid>();
            state.EntityManager.SetComponentData(singleton, new AvoidanceGrid()
            {
                agentsPerCell = new NativeParallelMultiHashMap<int, float3>(100, Allocator.Persistent)
            });

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

            var avoidanceGrid = SystemAPI.GetSingleton<AvoidanceGrid>();
            avoidanceGrid.agentsPerCell.Clear();
            if (entitiesCount > avoidanceGrid.agentsPerCell.Capacity)
            {
                avoidanceGrid.agentsPerCell.Capacity = entitiesCount * 4;
            }

            const int AVOIDANCE_GRID_CELL_SIZE = 2;
            SplitAgentsHandle = new SplitAgentsIntoCellsJob
            {
                cellSize = AVOIDANCE_GRID_CELL_SIZE,
                hashMapWriter = avoidanceGrid.agentsPerCell.AsParallelWriter(),
            }.ScheduleParallel(state.Dependency);

            var avoidanceHandle = new CalculateClosestAvoidanceVectorJob
            {
                cellSize = AVOIDANCE_GRID_CELL_SIZE,
                agentsPerCell = avoidanceGrid.agentsPerCell,
            }.ScheduleParallel(SplitAgentsHandle);
            
            state.Dependency = JobHandle.CombineDependencies(state.Dependency, avoidanceHandle);
        }

        public static int GetHashForPosition(in float2 position, in int cellSize)
        {
            var x = (int)math.round(position.x / cellSize);
            var y = (int)math.round(position.y / cellSize);
            return GridUtils.GetCellIndex(new int2(1000, 1000), x, y);
        }
    }
}