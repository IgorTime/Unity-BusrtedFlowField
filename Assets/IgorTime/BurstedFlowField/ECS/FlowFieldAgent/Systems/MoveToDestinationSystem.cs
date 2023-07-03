﻿using IgorTime.BurstedFlowField.ECS.Data;
using IgorTime.BurstedFlowField.ECS.Systems;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Systems
{
    [BurstCompile]
    [UpdateAfter(typeof(AgentAvoidanceSystem))]
    [UpdateInGroup(typeof(FlowFieldSystemGroup))]
    public partial struct MoveToDestinationSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<FlowFieldData>();
            state.RequireForUpdate<VectorFieldData>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var vectorField = SystemAPI.GetSingleton<VectorFieldData>();
            if (!vectorField.isSet)
            {
                return;
            }

            var gridData = SystemAPI.GetSingleton<FlowFieldData>();
            var j2 = new MoveToDestinationCellJob
            {
                dt = SystemAPI.Time.DeltaTime,
                grid = gridData,
                vectorField = vectorField.value,
            };

            j2.ScheduleParallel();
        }
    }
}