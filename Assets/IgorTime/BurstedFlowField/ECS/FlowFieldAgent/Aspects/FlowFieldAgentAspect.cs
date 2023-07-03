using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Aspects
{
    public readonly partial struct FlowFieldAgentAspect : IAspect
    {
        public readonly Entity Self;

        private readonly RefRO<Speed> speed;
        private readonly RefRW<FlowFieldAgent> flowFieldAgent;
        private readonly RefRW<LocalTransform> transform;

        public float3 Position
        {
            get => transform.ValueRO.Position;
            set => transform.ValueRW.Position = value;
        }

        public float Speed => speed.ValueRO.value;
        public float Radius => flowFieldAgent.ValueRO.radius;
        public float AvoidanceRadius => flowFieldAgent.ValueRO.avoidanceRadius;

        public float3 AvoidanceVector
        {
            get => flowFieldAgent.ValueRO.avoidanceVector;
            set => flowFieldAgent.ValueRW.avoidanceVector = value;
        }

        public int AvoidanceCounter
        {
            get => flowFieldAgent.ValueRO.avoidanceCounter;
            set => flowFieldAgent.ValueRW.avoidanceCounter = value;
        }
    }
}