﻿using IgorTime.BurstedFlowField.ECS.Data;
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
                
                DebugExtension.DebugPoint(cellWorldPosition, Color.red, 0.5f);
                // DbgDraw.WireQuad(cellWorldPosition, Quaternion.identity, Vector3.one, Color.cyan);
            }
        }
    }
    
    public partial class DebugDrawDestinationCell2 : SystemBase
    {
        protected override void OnUpdate()
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

                var bounds = new Bounds()
                {
                    center = cellWorldPosition,
                    size = Vector3.one * flowFieldData.ValueRO.cellRadius * 2
                };
                
                DebugExtension.DebugBounds(bounds, Color.red);
                // DebugExtension.DebugPoint(cellWorldPosition, Color.red, 0.5f);
            }
        }
    }
}