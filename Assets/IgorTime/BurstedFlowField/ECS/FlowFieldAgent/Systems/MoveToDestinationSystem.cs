using IgorTime.BurstedFlowField.ECS.Data;
using IgorTime.BurstedFlowField.ECS.Systems;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Systems
{
    [BurstCompile]
    [UpdateAfter(typeof(AgentAvoidanceSystem))]
    [UpdateInGroup(typeof(FlowFieldSystemGroup))]
    public partial struct MoveToDestinationSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<FlowFieldData>();
            state.RequireForUpdate<VectorFieldData>();
            state.RequireForUpdate<DestinationCell>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var vectorField = SystemAPI.GetSingleton<VectorFieldData>();
            if (!vectorField.isSet)
            {
                return;
            }

            var gridData = SystemAPI.GetSingleton<FlowFieldData>();
            var destinationCellData = SystemAPI.GetSingleton<DestinationCell>();
            var destinationPosition = GridUtilsBursted.GetWorldPositionFromCell(
                gridData.cellRadius,
                destinationCellData.cellCoordinates);
            
            var j2 = new MoveToDestinationCellJob
            {
                destinationPosition = destinationPosition,
                dt = SystemAPI.Time.DeltaTime,
                grid = gridData,
                vectorField = vectorField.value,
            };

            j2.ScheduleParallel();
        }
    }
}