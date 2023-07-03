using IgorTime.BurstedFlowField.ECS.Data;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace IgorTime.BurstedFlowField.ECS.Systems
{
    [BurstCompile]
    public partial struct DebugDrawVectorField : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.Enabled = false;
        }

        public void OnUpdate(ref SystemState state)
        {
            foreach (var (vectorField, flowField) in SystemAPI.Query<
                         RefRO<VectorFieldData>,
                         RefRO<FlowFieldData>>())
            {
                for (var i = 0; i < flowField.ValueRO.cellsCount; i++)
                {
                    var cellCoords = GridUtils.GetCellCoordinates(
                        flowField.ValueRO.gridSize,
                        i);

                    var cellPosition = GridUtils.GetWorldPositionFromCell(
                        flowField.ValueRO.cellRadius,
                        cellCoords);

                    var vectorPacked = vectorField.ValueRO.value[i];
                    var vector = GridDirection.Unpack(vectorPacked).X0Y_Vector3().normalized;
                    var halfVector = vector * 0.5f;

                    RuntimeGizmos.DrawArrow(cellPosition - halfVector, vector, Color.magenta);
                }
            }
        }
    }
}