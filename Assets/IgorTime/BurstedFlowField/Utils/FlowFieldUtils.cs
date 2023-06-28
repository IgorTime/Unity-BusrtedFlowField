using IgorTime.BurstedFlowField.Jobs;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField
{
    public static class FlowFieldUtils
    {
        public static void CalculateFlowFieldDebug(in int2 gridSize,
            in int2 destinationCell,
            [ReadOnly] NativeArray<byte> costField,
            NativeArray<ushort> integrationField,
            NativeArray<byte> vectorField)
        {
            CreateJobs(
                gridSize,
                destinationCell,
                costField,
                integrationField,
                vectorField,
                out var j1,
                out var j2, 
                out var j3);

            var cellsCount = costField.Length;
            j1.Run(cellsCount);
            j2.Run();
            j3.Run(cellsCount);
        }

        public static JobHandle CalculateFlowField(
            in int2 gridSize,
            in int2 destinationCell,
            [ReadOnly] NativeArray<byte> costField,
            NativeArray<ushort> integrationField,
            NativeArray<byte> vectorField)
        {
            CreateJobs(
                gridSize,
                destinationCell,
                costField,
                integrationField,
                vectorField,
                out var j1,
                out var j2, 
                out var j3);

            j1 = new ResetIntegrationFieldJob
            {
                integrationField = integrationField
            };

            j2 = new CalculateIntegrationFieldJob
            {
                targetCell = destinationCell,
                gridSize = gridSize,
                costField = costField,
                integrationField = integrationField
            };

            j3 = new CalculateVectorFieldJob
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

        private static void CreateJobs(
            in int2 gridSize,
            in int2 destinationCell,
            [ReadOnly] NativeArray<byte> costField,
            NativeArray<ushort> integrationField,
            NativeArray<byte> vectorField,
            out ResetIntegrationFieldJob j1,
            out CalculateIntegrationFieldJob j2,
            out CalculateVectorFieldJob j3)
        {
            j1 = new ResetIntegrationFieldJob
            {
                integrationField = integrationField
            };

            j2 = new CalculateIntegrationFieldJob
            {
                targetCell = destinationCell,
                gridSize = gridSize,
                costField = costField,
                integrationField = integrationField
            };

            j3 = new CalculateVectorFieldJob
            {
                gridSize = gridSize,
                integrationField = integrationField,
                vectorField = vectorField
            };
        }
    }
}