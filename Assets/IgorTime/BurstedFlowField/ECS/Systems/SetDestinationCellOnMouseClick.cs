using IgorTime.BurstedFlowField.ECS.Data;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace IgorTime.BurstedFlowField.ECS.Systems
{
    [BurstCompile]
    public partial struct SetDestinationCellOnMouseClick : ISystem
    {
        private int layerMask;
        private Camera camera;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<DestinationCell>();
            state.RequireForUpdate<FlowFieldData>();
            layerMask = LayerMask.GetMask("Ground");
            camera = Camera.main;
        }

        public void OnUpdate(ref SystemState state)
        {
            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }

            var flowField = SystemAPI.GetSingleton<FlowFieldData>();
            var destinationCell = SystemAPI.GetSingletonRW<DestinationCell>();

            var mousePos = Input.mousePosition;
            var ray = camera.ScreenPointToRay(mousePos);

            if (!Physics.Raycast(ray, out var hit, 1000, layerMask))
            {
                return;
            }

            var coordinates = GridUtils.GetCellFromWorldPosition(
                hit.point,
                flowField.gridSize,
                flowField.cellRadius);

            destinationCell.ValueRW.isSet = true;
            destinationCell.ValueRW.cellCoordinates = coordinates;
        }
    }
}