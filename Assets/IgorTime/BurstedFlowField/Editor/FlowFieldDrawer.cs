using System;
using UnityEditor;
using UnityEngine;

namespace IgorTime.BurstedFlowField.Editor
{
    public class FlowFieldDrawer
    {
        private static GUIStyle defaultStyle = new()
        {
            fontSize = 20,
            alignment = TextAnchor.MiddleCenter,
            normal = new GUIStyleState
            {
                textColor = Color.white
            }
        };
        
        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
        private static void DrawGrid(FlowFieldAuthoring target, GizmoType gizmoType)
        {
            if (target.grid.cellPositions == null) return;

            DrawGridCells(target.grid, Color.green);

            switch (target.drawTarget)
            {
                case DrawTarget.Coordinates:
                    DrawGridCoordinates(target.grid);
                    break;
                case DrawTarget.CostField:
                    DrawCostField(target.grid);
                    break;
                case DrawTarget.IntegrationField:
                    DrawIntegrationField(target.grid);
                    break;
                case DrawTarget.VectorField:
                    DrawVectorField(target.grid);
                    break;
            }
        }

        private static void DrawVectorField(in FlowFieldGrid grid)
        {
            for (var i = 0; i < grid.cellsCount; i++)
            {
                var position = grid.cellPositions[i].X0Y();
                var direction = grid.GetFlowDirection(i).X0Y().normalized;
                var halfDirection = direction * 0.5f;
                ArrowGizmo.Draw(position - halfDirection, direction, Color.red);
            }
        }

        private static void DrawGridCoordinates(in FlowFieldGrid targetGrid)
        {
            for (var i = 0; i < targetGrid.cellsCount; i++)
            {
                var position = targetGrid.cellPositions[i].X0Y();
                var coords = GridUtils.GetCellCoordinates(targetGrid.gridSize, i);
                Handles.Label(position, $"{coords.x}:{coords.y}", defaultStyle);
            }
        }

        private static void DrawIntegrationField(in FlowFieldGrid targetGrid)
        {
            if (!targetGrid.integrationField.IsCreated || 
                targetGrid.integrationField.Length == 0)
            {
                return;
            }
            
            for (var i = 0; i < targetGrid.cellsCount; i++)
            {
                var position = targetGrid.cellPositions[i].X0Y();
                Handles.Label(position, targetGrid.integrationField[i].ToString(), defaultStyle);
            }   
        }

        private static void DrawCostField(in FlowFieldGrid targetGrid)
        {
            if(targetGrid.costField == null) return;
            
            for (var i = 0; i < targetGrid.cellsCount; i++)
            {
                var t = targetGrid.costField[i] / CellCost.Max;
                var position = targetGrid.cellPositions[i].X0Y();
                var style = new GUIStyle()
                {
                    fontSize = 20,
                    alignment = TextAnchor.MiddleCenter,
                    normal = new GUIStyleState
                    {
                        textColor = Color.Lerp(Color.white, Color.red, t)
                    }
                };
                
                Handles.Label(position, targetGrid.costField[i].ToString(), style);
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