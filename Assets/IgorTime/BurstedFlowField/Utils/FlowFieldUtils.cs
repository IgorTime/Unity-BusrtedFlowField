using IgorTime.BurstedFlowField.Jobs;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField
{
    public static class FlowFieldUtils
    {
        public static JobHandle CalculateFlowField(
            in FlowFieldGrid grid,
            in int2 destinationCell)
        {
            return CalculateFlowField(
                grid.gridSize, 
                destinationCell,
                grid.costFieldUnmanaged,
                grid.integrationField,
                grid.vectorField);
        }

        public static JobHandle CalculateFlowField(
            in int2 gridSize,
            in int2 destinationCell,
            [ReadOnly] NativeArray<byte> costField,
            NativeArray<ushort> integrationField,
            NativeArray<byte> vectorField)
        {
            var j1 = new ResetIntegrationFieldJob
            {
                integrationField = integrationField
            };

            var j2 = new CalculateIntegrationFieldJob
            {
                targetCell = destinationCell,
                gridSize = gridSize,
                costField = costField,
                integrationField = integrationField
            };

            var j3 = new CalculateVectorFieldJob
            {
                gridSize = gridSize,
                integrationField = integrationField,
                vectorField = vectorField
            };

            var cellsCount = costField.Length;
            var h1 = j1.Schedule(cellsCount, 64);
            var h2 = j2.Schedule(h1);
            var h3 = j3.Schedule(cellsCount, 64, h2);
            return h3;
        }
    }
}