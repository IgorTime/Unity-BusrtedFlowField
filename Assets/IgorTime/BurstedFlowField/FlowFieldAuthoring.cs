using IgorTime.BurstedFlowField;
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

        flowFieldGrid.costFieldUnmanaged.CopyFrom(flowFieldGrid.costField);
    }
}