﻿using Unity.Entities;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Systems
{
    public readonly struct AvoidanceAgentData
    {
        public readonly Entity AgentEntity;
        public readonly float3 Position;
        public readonly float AgentRadius;

        public AvoidanceAgentData(in Entity agentEntity, in float3 position, in float agentRadius)
        {
            AgentEntity = agentEntity;
            Position = position;
            AgentRadius = agentRadius;
        }
    }
}