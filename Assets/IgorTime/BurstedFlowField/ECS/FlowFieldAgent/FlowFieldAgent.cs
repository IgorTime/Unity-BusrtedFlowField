using Unity.Entities;

namespace IgorTime.BurstedFlowField.ECS.FlowFieldAgent
{
    public struct FlowFieldAgent : IComponentData
    {
        public float radius;
        public float avoidanceRadius;
    }
}