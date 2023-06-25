using IgorTime.BurstedFlowField.ECS.Data;
using IgorTime.BurstedFlowField.Jobs;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField.ECS.Systems
{
    [BurstCompile]
    public partial struct CalculateFlowFieldSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (flowField, destinationCell) in SystemAPI.Query<
                         FlowFieldAspect,
                         RefRO<DestinationCell>>())
            {
                var targetCell = destinationCell.ValueRO.cellCoordinates;
                if (flowField.DestinationCell.Equals(targetCell)) continue;
                flowField.DestinationCell = targetCell;
                
                ResetIntegrationField(flowField);
                CalculateIntegrationField(destinationCell.ValueRO.cellCoordinates, flowField);
                CalculateVectorField(flowField);
            }
        }

        private void CalculateVectorField(in FlowFieldAspect flowField)
        {
            var calculateVectorFieldJob = new CalculateVectorFieldJob
            {
                gridSize = flowField.GridSize,
                integrationField = flowField.IntegrationField,
                vectorField = flowField.VectorField
            };

            calculateVectorFieldJob
                .Schedule(flowField.VectorField.Length, 64)
                .Complete();
        }

        private static void CalculateIntegrationField(in int2 targetCell, in FlowFieldAspect flowField)
        {
            var calculateIntegrationFieldJob = new CalculateIntegrationFieldJob
            {
                targetCell = targetCell,
                gridSize = flowField.GridSize,
                costField = flowField.CostField,
                integrationField = flowField.IntegrationField
            };

            calculateIntegrationFieldJob
                .Schedule()
                .Complete();
        }

        private static void ResetIntegrationField(in FlowFieldAspect flowField)
        {
            var resetIntegrationFieldJob = new ResetIntegrationFieldJob
            {
                integrationField = flowField.IntegrationField
            };

            resetIntegrationFieldJob
                .Schedule(flowField.IntegrationField.Length, 64)
                .Complete();
        }
    }
}