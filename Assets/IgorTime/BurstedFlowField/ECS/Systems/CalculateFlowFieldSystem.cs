using IgorTime.BurstedFlowField.ECS.Data;
using IgorTime.BurstedFlowField.Jobs;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace IgorTime.BurstedFlowField.ECS.Systems
{
    public partial struct CalculateFlowFieldSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var flowField in SystemAPI.Query<FlowFieldAspect>())
            {
                ResetIntegrationField(flowField);

                var targetCell = GetTargetCell();

                CalculateIntegrationField(targetCell, flowField);
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

        private int2 GetTargetCell()
        {
            if (Input.GetKey(KeyCode.Alpha1)) return new int2(10, 10);

            return int2.zero;
        }
    }
}