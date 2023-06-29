using Unity.Collections;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace IgorTime.BurstedFlowField.Editor
{
    public class FlowFieldDrawer
    {
        private static readonly GUIStyle DefaultStyle = new()
        {
            fontSize = 20,
            alignment = TextAnchor.MiddleCenter,
            normal = new GUIStyleState
            {
                textColor = Color.white
            }
        };

        private static readonly GUIStyle RedStyle = new()
        {
            fontSize = 20,
            alignment = TextAnchor.MiddleCenter,
            normal = new GUIStyleState
            {
                textColor = Color.red
            }
        };

        private static readonly GUIStyle YellowStyle = new()
        {
            fontSize = 20,
            alignment = TextAnchor.MiddleCenter,
            normal = new GUIStyleState
            {
                textColor = Color.yellow
            }
        };

        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
        private static void DrawGrid(FlowFieldDebugger target, GizmoType gizmoType)
        {
            if (!target.enabled) return;

            if (target.drawCells)
                DrawGridCells(
                    target.EditorData.cellPositions,
                    target.EditorData.cellRadius,
                    Color.green);

            switch (target.drawTarget)
            {
                case DrawTarget.Coordinates:
                    DrawGridCoordinates(
                        target.EditorData.cellPositions,
                        target.EditorData.gridSize);
                    break;
                case DrawTarget.CostField:
                    DrawCostField(
                        target.EditorData.cellPositions,
                        target.EditorData.costField);
                    break;
                case DrawTarget.IntegrationField:
                    if (!target.RuntimeData.HasValue) return;
                    DrawIntegrationField(
                        target.EditorData.cellPositions,
                        target.RuntimeData.Value.integrationField);
                    break;
                case DrawTarget.VectorField:
                    if (!target.RuntimeData.HasValue) return;
                    DrawVectorField(
                        target.EditorData.cellPositions,
                        target.RuntimeData.Value.vectorField);
                    break;
            }
        }

        private static void DrawVectorField(float2[] cellPositions, NativeArray<byte> vectorField)
        {
            if (!vectorField.IsCreated || vectorField.Length == 0) return;

            for (var i = 0; i < cellPositions.Length; i++)
            {
                var position = cellPositions[i].X0Y_Vector3();
                var direction = GridDirection.Unpack(vectorField[i]).X0Y_Vector3().normalized;
                var halfDirection = direction * 0.5f;
                ArrowGizmo.Draw(position - halfDirection, direction, Color.red);
            }
        }

        private static void DrawGridCoordinates(float2[] cellPositions, int2 gridSize)
        {
            for (var i = 0; i < cellPositions.Length; i++)
            {
                var position = cellPositions[i].X0Y_Vector3();
                var coords = GridUtils.GetCellCoordinates(gridSize, i);
                Handles.Label(position, $"{coords.x}:{coords.y}", DefaultStyle);
            }
        }

        private static void DrawIntegrationField(
            float2[] cellPositions,
            NativeArray<ushort> integrationField)
        {
            if (!integrationField.IsCreated ||
                integrationField.Length == 0)
                return;

            for (var i = 0; i < cellPositions.Length; i++)
            {
                var position = cellPositions[i].X0Y_Vector3();
                Handles.Label(position, integrationField[i].ToString(), DefaultStyle);
            }
        }

        private static void DrawCostField(float2[] cellPositions, byte[] costField)
        {
            if (costField == null) return;

            for (var i = 0; i < cellPositions.Length; i++)
            {
                var t = costField[i] / CellCost.Max;
                var position = cellPositions[i].X0Y_Vector3();

                var style = costField[i] == CellCost.Max
                    ? RedStyle
                    : costField[i] == CellCost.Default
                        ? DefaultStyle
                        : YellowStyle;

                Handles.Label(position, costField[i].ToString(), style);
            }
        }

        private static void DrawGridCells(float2[] cellPositions, float cellRadius, in Color color)
        {
            var previousColor = Gizmos.color;
            Gizmos.color = color;
            var cellSize = cellRadius * 2 * Vector3.one;
            for (var i = 0; i < cellPositions.Length; i++)
            {
                var cellPosition = cellPositions[i].X0Y_Vector3();
                Gizmos.DrawWireCube(cellPosition, cellSize);
            }

            Gizmos.color = previousColor;
        }
    }
}