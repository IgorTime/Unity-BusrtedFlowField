﻿using IgorTime.BurstedFlowField.ECS.Data;
using Unity.Collections;
using Unity.Entities;

namespace IgorTime.BurstedFlowField.ECS
{
    public class FlowFieldBaker : Baker<FlowFieldAuthoring>
    {
        public override void Bake(FlowFieldAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new FlowFieldData
            {
                cellsCount = authoring.CellsCount,
                gridSize = authoring.GridSize,
                cellRadius = authoring.CellRadius
            });

            AddComponent(entity, new CostFieldData
            {
                value = new NativeArray<byte>(authoring.CostField, Allocator.Persistent)
            });

            AddComponent(entity, new IntegrationFieldData
            {
                value = new NativeArray<ushort>(authoring.CellsCount, Allocator.Persistent)
            });

            AddComponent(entity, new VectorFieldData
            {
                value = new NativeArray<byte>(authoring.CellsCount, Allocator.Persistent)
            });
            
            AddComponent(entity, new DestinationCell());
        }
    }
}