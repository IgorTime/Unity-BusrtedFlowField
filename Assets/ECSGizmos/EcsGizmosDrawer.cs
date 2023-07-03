using System;
using System.Collections.Generic;
using UnityEngine;

namespace IgorTime.BurstedFlowField.ECS
{
    public class EcsGizmosDrawer : MonoBehaviour
    {
        private static EcsGizmosDrawer instance;
        private readonly List<ArrowGizmoData> arrowGizmos = new();
        private readonly List<BoxGizmoData> cubeGizmos = new();
        private readonly List<BoxGizmoData> wireCubeGizmos = new();
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
            Instance.arrowGizmos.Add(new ArrowGizmoData(
                pos,
                direction,
                color ?? Color.white,
                arrowHeadLength,
                arrowHeadAngle));
        }

        public static void DrawCube(
            in Vector3 pos,
            in Vector3 size,
            in Color? color = null)
        {
            Instance.cubeGizmos.Add(new BoxGizmoData(
                pos,
                size,
                color ?? Color.green));
        }

        public static void DrawWireCube(
            in Vector3 pos,
            in Vector3 size,
            in Color? color = null)
        {
            Instance.wireCubeGizmos.Add(new BoxGizmoData(
                pos,
                size,
                color ?? Color.green));
        }

        public static void DrawAction(Action action)
        {
            instance.gizmoActions.Add(action);
        }

        private void OnDrawGizmos()
        {
            DrawAllArrows();
            DrawAllCubes();
            DrawAllWireCubes();
            DrawActions();
        }

        private void DrawAllCubes()
        {
            var count = cubeGizmos.Count;
            for (var i = 0; i < count; i++)
            {
                Gizmos.color = cubeGizmos[i].Color;
                Gizmos.DrawCube(
                    cubeGizmos[i].Position,
                    cubeGizmos[i].Size);
            }
            
            cubeGizmos.Clear();
        }

        private void DrawAllWireCubes()
        {
            var count = wireCubeGizmos.Count;
            for (var i = 0; i < count; i++)
            {
                Gizmos.color = wireCubeGizmos[i].Color;
                Gizmos.DrawWireCube(
                    wireCubeGizmos[i].Position,
                    wireCubeGizmos[i].Size);
            }
            
            wireCubeGizmos.Clear();
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