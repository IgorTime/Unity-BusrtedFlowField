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
        public float3 destinationPosition;

        [ReadOnly]
        public NativeArray<byte> vectorField;
        
        [ReadOnly]
        public NativeArray<byte> costField;

        public void Execute(FlowFieldAgentAspect agentAspect)
        {
            var position = agentAspect.Position;
            var distanceToDestination = math.distancesq(destinationPosition, position);
            if(distanceToDestination < AgentAvoidanceSystem.ARRIVAL_DISTANCE)
            {
                return;
            }
            
            var cellIndex = grid.GetCellIndexFromWorldPosition(position);
            var moveVector = GridDirection.UnpackAsMoveDirection(vectorField[cellIndex]);
            var frameSpeed = agentAspect.Speed * dt;
            var desiredVelocity = (frameSpeed * moveVector).X0Y_Float3();


            if (agentAspect.AvoidanceCounter > 0)
            {
                // var avoidancePower = 1 - math.length(agentAspect.AvoidanceVector) / agentAspect.AvoidanceRadius;
                // var avoidanceTranslation = math.normalize(agentAspect.AvoidanceVector) * frameSpeed;
                // desiredVelocity = math.lerp(desiredVelocity, avoidanceTranslation, avoidancePower);
    
                desiredVelocity += agentAspect.AvoidanceVector * frameSpeed;
                desiredVelocity /=2;
            }

            var newPosition = position + desiredVelocity;
            if(!grid.IsValidPosition(newPosition))
            {
                return;
            }
            
            var newCellIndex = grid.GetCellIndexFromWorldPosition(newPosition);
            // if(!grid.IsValidCell(newCellIndex))
            // {
            //     return;
            // }
            
            // if (costField[newCellIndex] == CellCost.Max)
            // {
            //     return;
            // }
            
            agentAspect.Position += desiredVelocity;
        }
    }
}