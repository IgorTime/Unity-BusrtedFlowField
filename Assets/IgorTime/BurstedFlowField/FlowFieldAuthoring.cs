using IgorTime.BurstedFlowField;
using UnityEngine;

public class FlowFieldAuthoring : MonoBehaviour
{
    public float cellRadius;
    public Vector2Int gridSize;
    
    public FlowFieldGrid grid;
    
    [ContextMenu(nameof(CreateGrid))]
    public void CreateGrid()
    {
        grid = FlowFieldGrid.CreateGrid(gridSize, cellRadius);
    }
}