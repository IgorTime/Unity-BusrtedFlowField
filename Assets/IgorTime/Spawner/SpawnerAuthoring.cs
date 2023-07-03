using Unity.Entities;
using UnityEngine;

namespace IgorTime
{
    public class SpawnerAuthoring : MonoBehaviour
    {
        public GameObject prefab;
        public int agentsCount = 1000;
    }

    public class SpawnerBaker : Baker<SpawnerAuthoring>
    {
        public override void Bake(SpawnerAuthoring authoring)
        {
            var entity = GetEntity(authoring.gameObject, TransformUsageFlags.None);
            AddComponent(entity, new SpawnerData
            {
                prefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic),
                agentsCount = authoring.agentsCount,
            });
        }
    }
}