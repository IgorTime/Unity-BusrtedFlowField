using System;
using IgorTime.BurstedFlowField;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class FlowFieldAuthoring : MonoBehaviour
{
    [SerializeField] public FlowFieldEditorData editorData = new();
    public LayerMask obstaclesMask;
    [NonSerialized] public FlowFieldGrid? runtimeData;

    public int CellsCount => runtimeData?.cellsCount ?? 0;
    public int2 GridSize => editorData?.gridSize ?? default;
    public byte[] CostField => editorData?.costField ?? Array.Empty<byte>();
    public float CellRadius => editorData?.cellRadius ?? 0f;


    [ContextMenu(nameof(CreateGrid))]
    public void CreateGrid()
    {
        if (runtimeData.HasValue) runtimeData.Value.Dispose();

        editorData.CalculateCellPositions();
        editorData.CalculateCostField(obstaclesMask);

        CreateRuntimeData();
    }

    public void CreateRuntimeData()
    {
        runtimeData = new FlowFieldGrid
        {
            cellRadius = editorData.cellRadius,
            cellsCount = editorData.cellPositions.Length,
            gridSize = editorData.gridSize,
            costField = new NativeArray<byte>(editorData.costField, Allocator.Persistent),
            integrationField = new NativeArray<ushort>(editorData.costField.Length, Allocator.Persistent),
            vectorField = new NativeArray<byte>(editorData.costField.Length, Allocator.Persistent)
        };
    }
}