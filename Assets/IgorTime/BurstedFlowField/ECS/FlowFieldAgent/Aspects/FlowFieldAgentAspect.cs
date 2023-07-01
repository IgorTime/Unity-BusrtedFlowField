using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Aspects
{
    public readonly partial struct FlowFieldAgentAspect : IAspect
    {
        public readonly Entity Self;
    
        private readonly RefRO<Speed> speed;
        private readonly RefRO<FlowFieldAgent> flowFieldTag;
        public readonly RefRW<LocalTransform> transform;

        public float3 Position
        {
            get => transform.ValueRO.Position;
            set => transform.ValueRW.Position = value;
        }

        public float Speed => speed.ValueRO.value;
        public float Radius => flowFieldTag.ValueRO.radius;
        public float AvoidanceRadius => flowFieldTag.ValueRO.avoidanceRadius;
    }
}