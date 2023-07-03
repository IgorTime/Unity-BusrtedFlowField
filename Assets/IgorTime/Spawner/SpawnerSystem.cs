using IgorTime.BurstedFlowField;
using IgorTime.BurstedFlowField.ECS.Data;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace IgorTime
{
    public partial struct SpawnerSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<FlowFieldData>();
        }

        public void OnUpdate(ref SystemState state)
        {
            if (!Input.GetKeyDown(KeyCode.Space))
            {
                return;
            }

            var spawned = false;
            var flowField = SystemAPI.GetSingleton<FlowFieldData>();
            var random = new Random(1);
            var gridMin = new float2(1, 1);
            var gridMax = new float2(
                (flowField.gridSize.x - 1) * flowField.cellRadius * 2,
                (flowField.gridSize.y - 1) * flowField.cellRadius * 2);

            foreach (var spawnerData in SystemAPI.Query<RefRO<SpawnerData>>())
            {
                spawned = true;
                for (var i = 0; i < spawnerData.ValueRO.agentsCount; i++)
                {
                    var entity = state.EntityManager.Instantiate(spawnerData.ValueRO.prefab);
                    var position = random.NextFloat2(gridMin, gridMax).X0Y_Float3();
                    state.EntityManager.SetComponentData(entity, LocalTransform.FromPosition(position));
                }
            }

            if (spawned)
            {
                state.Enabled = false;
            }
        }
    }
}