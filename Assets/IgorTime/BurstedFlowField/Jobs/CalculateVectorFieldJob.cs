using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace IgorTime.BurstedFlowField.Jobs
{
    [BurstCompile]
    public struct CalculateVectorFieldJob : IJobParallelFor
    {
        [ReadOnly]
        public int2 gridSize;
        
        [ReadOnly]
        public NativeArray<ushort> integrationField;

        [WriteOnly]
        public NativeArray<byte> vectorField;

        public void Execute(int index)
        {
            var neighbors = new UnsafeList<int>(8, Allocator.Temp);
            GridUtils.GetAllNeighbors(gridSize, index, ref neighbors);
            
            var bestCost = integrationField[index];
            var bestIndex = index;
            for (var i = 0; i < neighbors.Length; i++)
            {
                var neighbor = neighbors[i];
                var neighborCost = integrationField[neighbor];
                if(neighborCost < bestCost)
                {
                    bestCost = neighborCost;
                    bestIndex = neighbor;
                }
            }
            
            var originCoordinates = GridUtils.GetCellCoordinates(gridSize, index);
            var bestCoordinates = GridUtils.GetCellCoordinates(gridSize, bestIndex);
            var direction = bestCoordinates - originCoordinates;
            vectorField[index] = GridDirection.PackDirection(direction);
            neighbors.Dispose();
        }
    }
}