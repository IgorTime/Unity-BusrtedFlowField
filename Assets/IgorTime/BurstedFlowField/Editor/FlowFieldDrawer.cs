using UnityEditor;
using UnityEngine;

namespace IgorTime.BurstedFlowField.Editor
{
    public class FlowFieldDrawer
    {
        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
        private static void DrawGrid(FlowFieldAuthoring target, GizmoType gizmoType)
        {
            if (target.grid.cellPositions == null) return;

            DrawGridCells(target.grid, Color.green);
            DrawGridCostField(target.grid);
        }

        private static void DrawGridCostField(in FlowFieldGrid targetGrid)
        {
            for (var i = 0; i < targetGrid.cellsCount; i++)
            {
                var position = targetGrid.cellPositions[i].X0Y();
                Handles.Label(position, targetGrid.costField[i].ToString());
            }   
        }

        private static void DrawGridCells(in FlowFieldGrid grid, in Color color)
        {
            var previousColor = Gizmos.color;
            Gizmos.color = color;
            var cellSize = grid.cellRadius * 2 * Vector3.one;
            for (var i = 0; i < grid.cellPositions.Length; i++)
            {
                var cellPosition = grid.cellPositions[i].X0Y();
                Gizmos.DrawWireCube(cellPosition, cellSize);
            }

            Gizmos.color = previousColor;
        }
    }
}