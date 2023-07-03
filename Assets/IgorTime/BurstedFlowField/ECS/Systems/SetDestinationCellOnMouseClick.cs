using IgorTime.BurstedFlowField.ECS.Data;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace IgorTime.BurstedFlowField.ECS.Systems
{
    [BurstCompile]
    public partial struct SetDestinationCellOnMouseClick : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }

            foreach (var (destinationCell, flowField) in SystemAPI.Query<
                         RefRW<DestinationCell>,
                         RefRO<FlowFieldData>>())
            {
                var mousePos = Input.mousePosition;
                var ray = Camera.main.ScreenPointToRay(mousePos);
                var layerMask = LayerMask.GetMask("Ground");
                if (!Physics.Raycast(ray, out var hit, 1000, layerMask))
                {
                    continue;
                }

                var coordinates = GridUtils.GetCellFromWorldPosition(
                    hit.point,
                    flowField.ValueRO.gridSize,
                    flowField.ValueRO.cellRadius);

                destinationCell.ValueRW.isSet = true;
                destinationCell.ValueRW.cellCoordinates = coordinates;
            }
        }
    }
}