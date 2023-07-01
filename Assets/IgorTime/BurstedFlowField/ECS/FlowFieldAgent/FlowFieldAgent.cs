using Unity.Entities;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField.ECS.FlowFieldAgent
{
    public struct FlowFieldAgent : IComponentData
    {
        public float radius;
        public float avoidanceRadius;
        public int avoidanceCounter;
        public float3 avoidanceVector;
    }
}