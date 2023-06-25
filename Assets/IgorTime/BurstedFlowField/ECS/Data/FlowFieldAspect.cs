using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField.ECS.Data
{
    public readonly partial struct FlowFieldAspect : IAspect
    {
        public readonly Entity Self;

        private readonly RefRO<FlowFieldData> gridData;
        private readonly RefRO<CostFieldData> costField;
        private readonly RefRW<IntegrationFieldData> integrationField;
        private readonly RefRW<VectorFieldData> vectorField;
        public NativeArray<ushort> IntegrationField => integrationField.ValueRW.value;
        public NativeArray<byte> CostField => costField.ValueRO.value;
        public int2 GridSize => gridData.ValueRO.gridSize;
        public NativeArray<byte> VectorField => vectorField.ValueRW.value;
    }
}