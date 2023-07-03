using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace IgorTime.BurstedFlowField.Editor
{
    [CustomEditor(typeof(FlowFieldDebugger))]
    public class FlowFieldDebuggerCustomEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var flowField = (FlowFieldDebugger) target;
            var root = new VisualElement();
            root.Add(new PropertyField(serializedObject.FindProperty("drawCells")));
            root.Add(new PropertyField(serializedObject.FindProperty("drawTarget")));

            root.Add(new PropertyField(serializedObject.FindProperty("targetCell")));
            root.Add(new Button(flowField.CalculateVectorField) {text = "Calculate Vector Field"});
            return root;
        }
    }
}