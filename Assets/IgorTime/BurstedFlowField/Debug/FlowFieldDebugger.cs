using System.Diagnostics;
using IgorTime.BurstedFlowField;
using Unity.Mathematics;
using UnityEngine;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(FlowFieldAuthoring))]
public class FlowFieldDebugger : MonoBehaviour
{
    public bool drawCells;
    public DrawTarget drawTarget;
    public int2 targetCell;

    private FlowFieldAuthoring flowFieldAuthoring;

    public FlowFieldEditorData EditorData => FlowFieldAuthoring.editorData;
    public FlowFieldGrid? RuntimeData => FlowFieldAuthoring.runtimeData;

    private FlowFieldAuthoring FlowFieldAuthoring =>
        flowFieldAuthoring
            ? flowFieldAuthoring
            : flowFieldAuthoring = GetComponent<FlowFieldAuthoring>();

    public void CalculateVectorField()
    {
        flowFieldAuthoring.CreateRuntimeData();

        if (!FlowFieldAuthoring.runtimeData.HasValue)
        {
            return;
        }

        var runtimeData = FlowFieldAuthoring.runtimeData.Value;

        var sw = Stopwatch.StartNew();
        // var h = FlowFieldUtils.CalculateFlowField(
        //     runtimeData.gridSize,
        //     targetCell,
        //     runtimeData.costField,
        //     runtimeData.integrationField,
        //     runtimeData.vectorField);
        //
        // h.Complete();

        FlowFieldUtils.CalculateFlowFieldDebug(
            runtimeData.gridSize,
            targetCell,
            runtimeData.costField,
            runtimeData.integrationField,
            runtimeData.vectorField);

        sw.Stop();
        var seconds = sw.Elapsed.TotalSeconds;
        var ms = sw.Elapsed.TotalMilliseconds;
        var ticks = sw.ElapsedTicks;

        Debug.Log("Flow field calculated in " + seconds + " seconds, " + ms + " ms, " + ticks + " ticks");
    }
}