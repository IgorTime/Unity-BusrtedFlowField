using IgorTime.BurstedFlowField;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

public class FlowFieldAuthoring : MonoBehaviour
{
    public float cellRadius;
    public FlowFieldGrid grid;
    public int2 gridSize;
    public LayerMask obstaclesMask;

    [Header("Debug")] 
    public DrawTarget drawTarget;
    public int2 destinationCell;
    
    [ContextMenu(nameof(CreateGrid))]
    public void CreateGrid()
    {
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

    [ContextMenu(nameof(CalculateIntegrationField))]
    public void CalculateIntegrationField()
    {
        CalculateIntegrationField(ref grid, destinationCell);
    }

    public void CalculateIntegrationField(ref FlowFieldGrid grid, in int2 destination)
    {
        ResetIntegrationField(ref grid);

        var destinationCellIndex = grid.GetCellIndex(destination);
        grid.integrationField[destinationCellIndex] = 0;

        var cellQueue = new UnsafeQueue<int>(Allocator.Temp);
        var neighbors = new UnsafeList<int>(4, Allocator.Temp);

        cellQueue.Enqueue(destinationCellIndex);
        while (cellQueue.Count > 0)
        {
            var currentCell = cellQueue.Dequeue();
            var cellIntegrationCost = grid.integrationField[currentCell];

            GridUtils.GetNeighbors(grid.gridSize, currentCell, ref neighbors);
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

    private void ResetIntegrationField(ref FlowFieldGrid flowFieldGrid)
    {
        for (var i = 0; i < flowFieldGrid.cellsCount; i++) flowFieldGrid.integrationField[i] = ushort.MaxValue;
    }
}

public enum DrawTarget
{
    CostField,
    IntegrationField,
    Coordinates
}