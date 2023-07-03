using UnityEngine;

namespace IgorTime.BurstedFlowField.ECS
{
    public readonly struct ArrowGizmoData
    {
        public readonly Vector3 Position;
        public readonly Vector3 Direction;
        public readonly Color Color;
        public readonly float ArrowHeadLength;
        public readonly float ArrowHeadAngle;
        
        public ArrowGizmoData(
            in Vector3 position,
            in Vector3 direction,
            in Color color,
            in float arrowHeadLength,
            in float arrowHeadAngle)
        {
            Position = position;
            Direction = direction;
            Color = color;
            ArrowHeadLength = arrowHeadLength;
            ArrowHeadAngle = arrowHeadAngle;
        }
    }
}