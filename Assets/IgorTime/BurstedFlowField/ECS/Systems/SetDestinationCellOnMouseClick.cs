using IgorTime.BurstedFlowField.ECS.Data;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace IgorTime.BurstedFlowField.ECS.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial class SetDestinationCellOnMouseClick : SystemBase
    {
        private int layerMask;
        private Camera camera;

        protected override void OnCreate()
        {
            RequireForUpdate<DestinationCell>();
            RequireForUpdate<FlowFieldData>();
            layerMask = LayerMask.GetMask("Ground");
            camera = Camera.main;
        }

        protected override void OnUpdate()
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