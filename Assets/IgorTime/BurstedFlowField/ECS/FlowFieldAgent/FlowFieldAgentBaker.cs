using Unity.Entities;

namespace IgorTime.BurstedFlowField.ECS.FlowFieldAgent
{
    public class FlowFieldAgentBaker : Baker<FlowFieldAgentAuthoring>
    {
        public override void Bake(FlowFieldAgentAuthoring authoring)
        {
            var entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);

            AddComponent(entity, new Speed
            {
                value = authoring.speed
            });
            
            AddComponent<FlowFieldAgentTag>(entity);
        }
    }
}