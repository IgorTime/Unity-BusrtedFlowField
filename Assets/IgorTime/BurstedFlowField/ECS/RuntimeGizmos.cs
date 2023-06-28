using System;
using System.Collections.Generic;
using UnityEngine;

namespace IgorTime.BurstedFlowField.ECS
{
    public class RuntimeGizmos : MonoBehaviour
    {
        private static RuntimeGizmos instance;
        private readonly List<Action> gizmoActions = new();

        private static RuntimeGizmos Instance
        {
            get
            {
                if (instance == null) instance = FindObjectOfType<RuntimeGizmos>();

                if (instance == null) instance = new GameObject("RuntimeGizmosManager").AddComponent<RuntimeGizmos>();

                return instance;
            }
        }

        public static void DrawArrow(
            Vector3 pos,
            Vector3 direction,
            Color? color,
            float arrowHeadLength = 0.2f,
            float arrowHeadAngle = 20f)
        {
            Instance.gizmoActions.Add(() =>
            {
                ArrowGizmo.Draw(pos, direction, color, arrowHeadLength, arrowHeadAngle);
            });
        }

        private void OnDrawGizmos()
        {
            foreach (var action in gizmoActions) action?.Invoke();

            gizmoActions.Clear();
        }
    }
}