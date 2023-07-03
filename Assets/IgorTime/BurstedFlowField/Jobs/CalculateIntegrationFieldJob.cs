using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField.Jobs
{
    [BurstCompile]
    public struct CalculateIntegrationFieldJob : IJob
    {
        [ReadOnly]
        public int2 targetCell;

        [ReadOnly]
        public int2 gridSize;

        [ReadOnly]
        public NativeArray<byte> costField;

        public NativeArray<ushort> integrationField;

        public void Execute()
        {
            var targetCellIndex = GridUtils.GetCellIndex(gridSize, targetCell);
            integrationField[targetCellIndex] = 0;

            var cellQueue = new UnsafeQueue<int>(Allocator.Temp);
            var neighbors = new UnsafeList<int>(4, Allocator.Temp);

            cellQueue.Enqueue(targetCellIndex);
            while (cellQueue.Count > 0)
            {
                var currentCell = cellQueue.Dequeue();
                var cellIntegrationCost = integrationField[currentCell];

                GridUtils.GetCardinalNeighbors(gridSize, currentCell, ref neighbors);

                for (var i = 0; i < neighbors.Length; i++)
                {
                    var neighbor = neighbors[i];
                    var neighborCost = costField[neighbor];
                    if (neighborCost >= CellCost.Max)
                    {
                        continue;
                    }

                    if (neighborCost + cellIntegrationCost < integrationField[neighbor])
                    {
                        integrationField[neighbor] = (ushort) (neighborCost + cellIntegrationCost);
                        cellQueue.Enqueue(neighbor);
                    }
                }
            }
        }
    }
}