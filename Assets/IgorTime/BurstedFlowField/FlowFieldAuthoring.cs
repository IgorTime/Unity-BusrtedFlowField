using IgorTime.BurstedFlowField;
using Unity.Mathematics;
using UnityEngine;

public class FlowFieldAuthoring : MonoBehaviour
{
    public float cellRadius;

    public FlowFieldGrid grid;
    public Vector2Int gridSize;
    public LayerMask obstaclesMask;

    [ContextMenu(nameof(CreateGrid))]
    public void CreateGrid()
    {
        grid = FlowFieldGrid.CreateGrid(gridSize, cellRadius);
        grid.costField = CalculateCostField(grid);
    }

    private byte[] CalculateCostField(in FlowFieldGrid flowFieldGrid)
    {
        var result = new byte[flowFieldGrid.cellsCount];
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
            
            result[i] = hits > 0 ? CellCost.Max : CellCost.Default;
        }

        return result;
    }
}