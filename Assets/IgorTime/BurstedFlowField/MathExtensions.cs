using Unity.Mathematics;
using UnityEngine;

namespace IgorTime.BurstedFlowField
{
    public static class MathExtensions
    {
        public static Vector3 X0Y(this in float2 float2)
        {
            return new Vector3(float2.x, 0, float2.y);
        }
    }
}