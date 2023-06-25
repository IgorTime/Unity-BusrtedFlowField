﻿using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField.ECS.Data
{
    [ChunkSerializable]
    public struct VectorFieldData : IComponentData
    {
        public int2 destinationCell;
        public NativeArray<byte> value;
    }
}