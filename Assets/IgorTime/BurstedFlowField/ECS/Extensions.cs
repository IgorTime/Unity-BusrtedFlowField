using IgorTime.BurstedFlowField.ECS.Data;
using Unity.Burst;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField.ECS
{
    public static class Extensions
    {
        [BurstCompile]
        public static int2 GetCellFromWorldPosition(
            this in FlowFieldData ffData,
            in float3 position) =>
            GridUtils.GetCellFromWorldPosition(
                position,
                ffData.gridSize,
                ffData.cellRadius);

        [BurstCompile]
        public static int GetCellIndexFromWorldPosition(
            this in FlowFieldData ffData,
            in float3 position) =>
            GridUtils.GetCellIndexFromWorldPosition(
                position,
                ffData.gridSize,
                ffData.cellRadius);
    }
}