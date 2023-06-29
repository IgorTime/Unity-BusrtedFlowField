using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace IgorTime.BurstedFlowField.ECS.FlowFieldAgent.Aspects
{
    public readonly partial struct FlowFieldAgentAspect : IAspect
    {
        public readonly Entity Self;

        private readonly RefRO<Speed> speed;
        private readonly RefRO<FlowFieldAgentTag> flowFieldTag;
        private readonly RefRW<LocalTransform> transform;
        
        public ref float3 Position => ref transform.ValueRW.Position;

        public float Speed => speed.ValueRO.value;
    }
}