using IgorTime.BurstedFlowField.ECS.Data;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace IgorTime.BurstedFlowField.ECS.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(DrawDebugGizmosSystemGroup))]
    public partial struct DebugDrawDestinationCell : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<FlowFieldData>();
            state.RequireForUpdate<DestinationCell>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var destinationCell = SystemAPI.GetSingleton<DestinationCell>();
            var flowFieldData = SystemAPI.GetSingleton<FlowFieldData>();

            if (!destinationCell.isSet)
            {
                return;
            }

            var cellCoordinates = destinationCell.cellCoordinates;
            var cellWorldPosition = GridUtils.GetWorldPositionFromCell(
                flowFieldData.cellRadius,
                cellCoordinates);

            EcsGizmosDrawer.DrawCube(cellWorldPosition, Vector3.one * flowFieldData.cellRadius * 2);
        }
    }
}