using IgorTime.BurstedFlowField.ECS.Data;
using IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Aspects;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Systems
{
    [BurstCompile]
    public partial struct MoveToDestinationCellJob : IJobEntity
    {
        public float dt;
        public FlowFieldData grid;
        [ReadOnly] public NativeArray<byte> vectorField;

        public void Execute(FlowFieldAgentAspect agentAspect)
        {
            var position = agentAspect.Position;
            var cellIndex = grid.GetCellIndexFromWorldPosition(position);
            var moveVector = GridDirection.Unpack(vectorField[cellIndex]);
            var moveDirection = math.normalize(moveVector);

            ref var agentPosition = ref agentAspect.Position;
            var translation = (agentAspect.Speed * dt * moveDirection).X0Y_Float3();
            agentPosition += translation;
        }
    }
}