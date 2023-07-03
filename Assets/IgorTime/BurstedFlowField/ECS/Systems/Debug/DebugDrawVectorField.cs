using IgorTime.BurstedFlowField.ECS.Data;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace IgorTime.BurstedFlowField.ECS.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(DrawDebugGizmosSystemGroup))]
    public partial struct DebugDrawVectorField : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<VectorFieldData>();
            state.RequireForUpdate<FlowFieldData>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var flowField = SystemAPI.GetSingleton<FlowFieldData>();
            var vectorField = SystemAPI.GetSingleton<VectorFieldData>();

            for (var i = 0; i < flowField.cellsCount; i++)
            {
                var cellCoords = GridUtils.GetCellCoordinates(
                    flowField.gridSize,
                    i);

                var cellPosition = GridUtils.GetWorldPositionFromCell(
                    flowField.cellRadius,
                    cellCoords);

                var vectorPacked = vectorField.value[i];
                var vector = GridDirection.UnpackAsMoveDirection(vectorPacked).X0Y_Vector3();
                var halfVector = vector * 0.5f;

                EcsGizmosDrawer.DrawArrow(cellPosition - halfVector, vector, Color.magenta);
            }
        }
    }
}