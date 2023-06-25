using Unity.Collections;
using Unity.Entities;

namespace IgorTime.BurstedFlowField.ECS.Data
{
    [ChunkSerializable]
    public struct VectorFieldData : IComponentData
    {
        public NativeArray<byte> value;
    }
}