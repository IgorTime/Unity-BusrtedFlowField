using IgorTime.BurstedFlowField;
using IgorTime.BurstedFlowField.Jobs;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class FlowFieldAuthoring : MonoBehaviour
{
    public float cellRadius;
    public int2 gridSize;
    public LayerMask obstaclesMask;

    public FlowFieldGrid grid;


    public int CellsCount => grid.cellsCount;
    public int2 GridSize => gridSize;
    public float2[] CellPositions => grid.cellPositions;
    public byte[] CostField => grid.costField;

    [ContextMenu(nameof(CreateGrid))]
    public void CreateGrid()
    {
        grid.Dispose();
        grid = FlowFieldGrid.CreateGrid(gridSize, cellRadius);
        CalculateCostField(ref grid);
    }

    private void CalculateCostField(ref FlowFieldGrid flowFieldGrid)
    {
        var collidersBuffer = new Collider[10];
        var halfExtends = flowFieldGrid.cellRadius * Vector3.one;
        for (var i = 0; i < flowFieldGrid.cellsCount; i++)
        {
            var cellPosition = flowFieldGrid.cellPositions[i];
            var hits = Physics.OverlapBoxNonAlloc(
                cellPosition.X0Y(),
                halfExtends,
                collidersBuffer,
                quaternion.identity,
                obstaclesMask);

            flowFieldGrid.costField[i] = hits > 0 ? CellCost.Max : CellCost.Default;
        }
    }

    public void CalculateIntegrationField(ref FlowFieldGrid grid, in int2 destination)
    {
        ResetIntegrationFieldParallel(ref grid);

        var destinationCellIndex = grid.GetCellIndex(destination);
        grid.integrationField[destinationCellIndex] = 0;

        var cellQueue = new UnsafeQueue<int>(Allocator.Temp);
        var neighbors = new UnsafeList<int>(4, Allocator.Temp);

        cellQueue.Enqueue(destinationCellIndex);
        while (cellQueue.Count > 0)
        {
            var currentCell = cellQueue.Dequeue();
            var cellIntegrationCost = grid.integrationField[currentCell];

            grid.GetCardinalNeighbors(currentCell, ref neighbors);

            for (var i = 0; i < neighbors.Length; i++)
            {
                var neighbor = neighbors[i];
                var neighborCost = grid.costField[neighbor];
                if (neighborCost >= CellCost.Max) continue;

                if (neighborCost + cellIntegrationCost < grid.integrationField[neighbor])
                {
                    grid.integrationField[neighbor] = (ushort)(neighborCost + cellIntegrationCost);
                    cellQueue.Enqueue(neighbor);
                }
            }
        }
    }

    public void CalculateVectorField()
    {
        var job = new CalculateVectorFieldJob
        {
            gridSize = grid.gridSize,
            integrationField = grid.integrationField,
            vectorField = grid.vectorField
        };

        job.Schedule(grid.cellsCount, 64).Complete();
    }

    private void ResetIntegrationFieldParallel(ref FlowFieldGrid flowFieldGrid)
    {
        var job = new ResetIntegrationFieldJob
        {
            integrationField = flowFieldGrid.integrationField
        };

        job.Schedule(flowFieldGrid.cellsCount, 64).Complete();
    }
}