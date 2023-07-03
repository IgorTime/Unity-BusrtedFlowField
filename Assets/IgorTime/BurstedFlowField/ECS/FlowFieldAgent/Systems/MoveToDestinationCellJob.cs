using IgorTime.BurstedFlowField.ECS.Data;
using IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Aspects;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Systems
{
    [BurstCompile]
    [UpdateAfter(typeof(AgentAvoidanceSystem))]
    public partial struct MoveToDestinationCellJob : IJobEntity
    {
        public float dt;
        public FlowFieldData grid;

        [ReadOnly]
        public NativeArray<byte> vectorField;

        public void Execute(FlowFieldAgentAspect agentAspect)
        {
            var position = agentAspect.Position;
            var cellIndex = grid.GetCellIndexFromWorldPosition(position);
            var moveVector = GridDirection.UnpackAsMoveDirection(vectorField[cellIndex]);
            var frameSpeed = agentAspect.Speed * dt;
            var translation = (frameSpeed * moveVector).X0Y_Float3();

            if (agentAspect.AvoidanceCounter > 0)
            {
                var avoidancePower = 1 - math.length(agentAspect.AvoidanceVector) / agentAspect.AvoidanceRadius;
                var avoidanceTranslation = math.normalize(agentAspect.AvoidanceVector) * frameSpeed;
                translation = math.lerp(translation, avoidanceTranslation, avoidancePower);
            }

            agentAspect.Position += translation;
        }
    }
}