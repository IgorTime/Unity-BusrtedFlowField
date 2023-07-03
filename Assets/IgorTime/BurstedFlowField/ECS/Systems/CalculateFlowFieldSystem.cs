using IgorTime.BurstedFlowField.ECS.Data;
using Unity.Burst;
using Unity.Entities;

namespace IgorTime.BurstedFlowField.ECS.Systems
{
    [BurstCompile]
    [UpdateAfter(typeof(SetDestinationCellOnMouseClick))]
    public partial struct CalculateFlowFieldSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (flowField, destinationCell) in SystemAPI.Query<
                         FlowFieldAspect,
                         RefRO<DestinationCell>>())
            {
                var targetCell = destinationCell.ValueRO.cellCoordinates;
                if (flowField.DestinationCell.Equals(targetCell))
                {
                    continue;
                }

                flowField.DestinationCell = targetCell;

                FlowFieldUtils.CalculateFlowField(
                                   flowField.GridSize,
                                   flowField.DestinationCell,
                                   flowField.CostField,
                                   flowField.IntegrationField,
                                   flowField.VectorField)
                              .Complete();

                flowField.IsSet = true;
            }
        }
    }
}