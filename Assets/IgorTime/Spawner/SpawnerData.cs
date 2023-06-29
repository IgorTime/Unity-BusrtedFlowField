using Unity.Entities;

namespace IgorTime
{
    public struct SpawnerData : IComponentData
    {
        public Entity prefab;
        public int agentsCount;
    }
}