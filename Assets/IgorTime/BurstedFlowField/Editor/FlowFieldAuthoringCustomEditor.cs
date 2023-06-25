using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace IgorTime.BurstedFlowField.Editor
{
    [CustomEditor(typeof(FlowFieldAuthoring))]
    public class FlowFieldAuthoringCustomEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var flowField = (FlowFieldAuthoring)target;
            var root = new VisualElement();
            root.Add(new PropertyField(serializedObject.FindProperty(nameof(FlowFieldAuthoring.cellRadius))));
            root.Add(new PropertyField(serializedObject.FindProperty(nameof(FlowFieldAuthoring.gridSize))));
            root.Add(new PropertyField(serializedObject.FindProperty(nameof(FlowFieldAuthoring.obstaclesMask))));
            root.Add(new Button(flowField.CreateGrid) { text = "Create Grid" });
            return root;
        }
    }
}