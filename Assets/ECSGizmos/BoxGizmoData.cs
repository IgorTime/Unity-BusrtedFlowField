using UnityEngine;

namespace IgorTime.BurstedFlowField.ECS
{
    public readonly struct BoxGizmoData
    {
        public readonly Vector3 Position;
        public readonly Vector3 Size;
        public readonly Color Color;

        public BoxGizmoData(
            in Vector3 position,
            in Vector3 size,
            in Color color)
        {
            Position = position;
            Size = size;
            Color = color;
        }
    }
}