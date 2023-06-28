using System;
using Unity.Collections;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField
{
    [Serializable]
    public struct FlowFieldGrid : IDisposable
    {
        public int cellsCount;
        public float cellRadius;
        public int2 gridSize;

        public NativeArray<byte> costField;
        public NativeArray<ushort> integrationField;
        public NativeArray<byte> vectorField;

        public void Dispose()
        {
            costField.Dispose();
            integrationField.Dispose();
            vectorField.Dispose();
        }
    }
}