using IgorTime.BurstedFlowField;
using UnityEngine;

[RequireComponent(typeof(FlowFieldAuthoring))]
public class FlowFieldDebugger : MonoBehaviour
{
    public DrawTarget drawTarget;
    public bool drawCells;

    private FlowFieldAuthoring flowFieldAuthoring;

    private FlowFieldAuthoring FlowFieldAuthoring => flowFieldAuthoring
        ? flowFieldAuthoring
        : flowFieldAuthoring = GetComponent<FlowFieldAuthoring>();

    public ref FlowFieldGrid GridRef => ref FlowFieldAuthoring.grid;
}