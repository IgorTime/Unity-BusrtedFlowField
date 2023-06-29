using IgorTime.BurstedFlowField.ECS.Data;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace IgorTime
{
    public partial struct SpawnerSystem : ISystem
    {
        // public void OnCreate(ref SystemState state)
        // {
        //     using var ecb = new EntityCommandBuffer(Allocator.Temp);
        //     var spawnerData = SystemAPI.GetSingleton<SpawnerData>();
        //     var flowField = SystemAPI.GetSingleton<FlowFieldData>();
        //     
        //     for (var i = 0; i < spawnerData.agentsCount; i++)
        //     {
        //         ecb.Instantiate(spawnerData.prefab);
        //     }
        //     
        //     ecb.Playback(state.EntityManager);
        // }
        
        public void OnUpdate(ref SystemState state)
        {
            var spawned = false;
            using var ecb = new EntityCommandBuffer(Allocator.Temp);
            var flowField = SystemAPI.GetSingleton<FlowFieldData>();
            var random = new Unity.Mathematics.Random(1);
            foreach (var spawnerData in SystemAPI.Query<RefRO<SpawnerData>>())
            {
                spawned = true;
                for (var i = 0; i < spawnerData.ValueRO.agentsCount; i++)
                {
                    var entity = ecb.Instantiate(spawnerData.ValueRO.prefab);
                    
                    var position = new float3(
                        random.NextInt(0, flowField.gridSize.x) * flowField.cellRadius,
                        0,
                        random.NextInt(0, flowField.gridSize.y)) * flowField.cellRadius;
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