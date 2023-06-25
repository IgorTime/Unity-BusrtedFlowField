using IgorTime.BurstedFlowField.ECS.Data;
using Oddworm.Framework;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace IgorTime.BurstedFlowField.ECS.Systems
{
    [BurstCompile]
    public partial struct DebugDrawDestinationCell : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (destinationCell, flowFieldData) in SystemAPI.Query<
                         RefRO<DestinationCell>, 
                         RefRO<FlowFieldData>>())
            {
                if (!destinationCell.ValueRO.isSet)
                {
                    continue;
                }
                
                var cellCoordinates = destinationCell.ValueRO.cellCoordinates;
                var cellWorldPosition = GridUtils.GetWorldPositionFromCell(
                    flowFieldData.ValueRO.cellRadius,
                    cellCoordinates);
                
                DbgDraw.WireQuad(cellWorldPosition, Quaternion.identity, Vector3.one, Color.cyan);
            }
        }
    }
}