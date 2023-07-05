using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace IgorTime.BurstedFlowField
{
    public static class MathExtensions
    {
        public static Vector3 X0Y_Vector3(this in float2 float2) => new(float2.x, 0, float2.y);

        public static Vector3 X0Y_Vector3(this in int2 int2) => new(int2.x, 0, int2.y);

        public static float3 X0Y_Float3(this in float2 float2) => new(float2.x, 0, float2.y);

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Squared(this in float value) => value * value;

        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 ClampLength(this in float3 value, in float clamp)
        {
            if (math.lengthsq(value) > clamp * clamp)
            {
                return math.normalize(value) * clamp;
            }

            return value;
        }
    }
}