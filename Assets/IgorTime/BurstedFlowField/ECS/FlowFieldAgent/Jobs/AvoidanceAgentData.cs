using Unity.Entities;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Systems
{
    public readonly struct AvoidanceAgentData
    {
        public readonly Entity AgentEntity;
        public readonly float3 Position;
        
        public AvoidanceAgentData(in Entity agentEntity, in float3 position)
        {
            AgentEntity = agentEntity;
            Position = position;
        }
    }
}