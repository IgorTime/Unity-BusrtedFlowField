using IgorTime.BurstedFlowField;
using IgorTime.BurstedFlowField.ECS.Data;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace IgorTime
{
    public partial struct SpawnerSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var spawned = false;
            using var ecb = new EntityCommandBuffer(Allocator.Temp);
            var flowField = SystemAPI.GetSingleton<FlowFieldData>();
            var random = new Random(1);
            var gridMin = new float2(0, 0);
            var gridMax = new float2(
                flowField.gridSize.x * flowField.cellRadius * 2, 
                flowField.gridSize.y * flowField.cellRadius * 2);
            
            foreach (var spawnerData in SystemAPI.Query<RefRO<SpawnerData>>())
            {
                spawned = true;
                for (var i = 0; i < spawnerData.ValueRO.agentsCount; i++)
                {
                    var entity = ecb.Instantiate(spawnerData.ValueRO.prefab);
                    var position = random.NextFloat2(gridMin, gridMax).X0Y_Float3();
                    ecb.SetComponent(entity, LocalTransform.FromPosition(position));
                }
            }

            if (spawned)
            {
                state.Enabled = false;
                ecb.Playback(state.EntityManager);
            }
        }
    }
}