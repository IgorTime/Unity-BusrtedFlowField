using Unity.Collections;
using Unity.Entities;

namespace IgorTime.BurstedFlowField.ECS.Data
{
    [ChunkSerializable]
    public struct IntegrationFieldData : IComponentData
    {
        public NativeArray<ushort> value;
    }
}