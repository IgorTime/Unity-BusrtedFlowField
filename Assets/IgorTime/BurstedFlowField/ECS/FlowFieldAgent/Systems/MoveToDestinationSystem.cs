using IgorTime.BurstedFlowField.ECS.Data;
using Unity.Burst;
using Unity.Entities;

namespace IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Systems
{
    [BurstCompile]
    public partial struct MoveToDestinationSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var gridData = SystemAPI.GetSingleton<FlowFieldData>();
            var vectorField = SystemAPI.GetSingleton<VectorFieldData>();
            
            var j2 = new MoveToDestinationCellJob
            {
                dt = SystemAPI.Time.DeltaTime,
                grid = gridData,
                vectorField = vectorField.value
            };

            j2.ScheduleParallel();
        }
    }
}