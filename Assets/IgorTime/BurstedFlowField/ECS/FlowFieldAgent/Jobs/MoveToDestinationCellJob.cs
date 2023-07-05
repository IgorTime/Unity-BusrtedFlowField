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

            float3 desiredVelocity;
            if (agentAspect.AvoidanceCounter > 0)
            {
                desiredVelocity = agentAspect.AvoidanceVector.ClampLength(agentAspect.Speed);
            }
            else
            {
                var cellIndex = grid.GetCellIndexFromWorldPosition(position);
                var moveVector = GridDirection.UnpackAsMoveDirection(vectorField[cellIndex]);
                desiredVelocity = (moveVector * agentAspect.Speed).X0Y_Float3();
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
            
            agentAspect.Position += desiredVelocity * dt;
        }
    }
}