using System;
using Unity.Mathematics;
using UnityEngine;

namespace IgorTime.BurstedFlowField
{
    [Serializable]
    public class FlowFieldEditorData
    {
        public float2[] cellPositions;
        public float cellRadius;

        public byte[] costField;
        public int2 gridSize;

        public void CalculateCellPositions()
        {
            cellPositions = GridUtils.GetCellPositions(gridSize, cellRadius);
        }

        public void CalculateCostField(LayerMask obstaclesMask)
        {
            var collidersBuffer = new Collider[10];
            var halfExtends = cellRadius * Vector3.one;
            var cellsCount = gridSize.x * gridSize.y;
            costField = new byte[cellsCount];
            for (var i = 0; i < cellsCount; i++)
            {
                var cellPosition = cellPositions[i];
                var hits = Physics.OverlapBoxNonAlloc(
                    cellPosition.X0Y(),
                    halfExtends,
                    collidersBuffer,
                    quaternion.identity,
                    obstaclesMask);

                costField[i] = hits > 0 ? CellCost.Max : CellCost.Default;
            }
        }
    }
}