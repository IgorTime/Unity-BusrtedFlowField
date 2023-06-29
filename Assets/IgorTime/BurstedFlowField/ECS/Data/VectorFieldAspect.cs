using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField.ECS.Data
{
    [BurstCompile]
    public readonly partial struct VectorFieldAspect : IAspect
    {
        private readonly RefRO<FlowFieldData> gridData;
        // private readonly RefRO<VectorFieldData> vectorField;

        public float2 GetMoveDirectionFromWorldPosition(in float3 position)
        {
            // var cellIndex = gridData.ValueRO.GetCellIndexFromWorldPosition(position);
            // var vectorPacked = vectorField.ValueRO.value[cellIndex];
            // return GridDirection.Unpack(vectorPacked);

            return new float2();
        }
    }
}