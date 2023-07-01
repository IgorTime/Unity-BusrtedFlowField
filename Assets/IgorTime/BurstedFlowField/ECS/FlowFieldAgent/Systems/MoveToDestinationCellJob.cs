using IgorTime.BurstedFlowField.ECS.Data;
using IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Aspects;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

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
            var moveVector = GridDirection.UnpackAsMoveDirection(vectorField[cellIndex]);
            var translation = (agentAspect.Speed * dt * moveVector).X0Y_Float3();
            agentAspect.Position += translation;
        }
    }
}