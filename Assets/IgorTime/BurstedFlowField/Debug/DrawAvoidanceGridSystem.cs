using IgorTime.BurstedFlowField.ECS;
using IgorTime.BurstedFlowField.ECS.Data;
using IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Systems;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace IgorTime.BurstedFlowField.Debug
{
    [UpdateInGroup(typeof(DrawDebugGizmosSystemGroup))]
    [UpdateBefore(typeof(AgentAvoidanceSystem))]
    public partial class DrawAvoidanceGridSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<AvoidanceGrid>();
        }

        protected override void OnUpdate()
        {
            var systemHandle = World.GetExistingSystem<AgentAvoidanceSystem>();
            var system = World.Unmanaged.GetUnsafeSystemRef<AgentAvoidanceSystem>(systemHandle);

            system.SplitAgentsHandle.Complete();

            var avoidanceGrid = SystemAPI.GetSingleton<AvoidanceGrid>();
            var agentsPerCell = avoidanceGrid.agentsPerCell;
            var (result, uniques) = agentsPerCell.GetUniqueKeyArray(Allocator.Temp);

            for (int i = 0; i < uniques; i++)
            {
                var key = result[i];
                foreach (var value in agentsPerCell.GetValuesForKey(key))
                {
                    var finalColor = key % 2 == 0 ? Color.red : Color.blue;
                    EcsGizmosDrawer.DrawCube(
                        value, 
                        Vector3.one * 0.5f * 0.5f, 
                        finalColor
                    );
                }
            }
        }
    }
}