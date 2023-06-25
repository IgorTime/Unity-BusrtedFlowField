using Unity.Collections;
using Unity.Entities;

namespace IgorTime.BurstedFlowField.ECS.Data
{
    [ChunkSerializable]
    public struct CostFieldData : IComponentData
    {
        public NativeArray<byte> value;
    }
}