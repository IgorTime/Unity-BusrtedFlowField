using System;
using System.Collections.Generic;
using UnityEngine;

namespace IgorTime.BurstedFlowField.ECS
{
    public class EcsGizmosDrawer : MonoBehaviour
    {
        private static EcsGizmosDrawer instance;
        private readonly List<ArrowGizmoData> arrowGizmos = new();
        private readonly List<Action> gizmoActions = new();

        private static EcsGizmosDrawer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<EcsGizmosDrawer>();
                }

                if (instance == null)
                {
                    instance = new GameObject("RuntimeGizmosManager").AddComponent<EcsGizmosDrawer>();
                }

                return instance;
            }
        }

        public static void DrawArrow(
            in Vector3 pos,
            in Vector3 direction,
            in Color? color = null,
            in float arrowHeadLength = 0.2f,
            in float arrowHeadAngle = 20f)
        {
            Instance.arrowGizmos.Add(new ArrowGizmoData
            {
                Position = pos,
                Direction = direction,
                Color = color ?? Color.white,
                ArrowHeadLength = arrowHeadLength,
                ArrowHeadAngle = arrowHeadAngle,
            });
        }
        
        public static void DrawAction(Action action)
        {
            instance.gizmoActions.Add(action);
        }

        private void OnDrawGizmos()
        {
            DrawAllArrows();
            DrawActions();
        }

        private void DrawActions()
        {
            foreach (var action in gizmoActions)
            {
                action?.Invoke();
            }

            gizmoActions.Clear();
        }

        private void DrawAllArrows()
        {
            var gizmosCount = arrowGizmos.Count;
            for (var i = 0; i < gizmosCount; i++)
            {
                ArrowGizmo.Draw(
                    arrowGizmos[i].Position,
                    arrowGizmos[i].Direction,
                    arrowGizmos[i].Color,
                    arrowGizmos[i].ArrowHeadLength,
                    arrowGizmos[i].ArrowHeadAngle);
            }

            arrowGizmos.Clear();
        }
    }
}