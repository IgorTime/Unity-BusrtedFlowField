using UnityEngine;

public static class ArrowGizmo
{
    public static void Draw(
        in Vector3 pos,
        in Vector3 direction,
        in Color? color = null,
        in float arrowHeadLength = 0.2f,
        in float arrowHeadAngle = 20.0f)
    {
        Gizmos.color = color ?? Color.white;
        Gizmos.DrawRay(pos, direction);

        if (direction == Vector3.zero)
        {
            return;
        }

        var right = Quaternion.LookRotation(direction) *
                    Quaternion.Euler(0, 180 + arrowHeadAngle, 0) *
                    Vector3.forward;

        var left = Quaternion.LookRotation(direction) *
                   Quaternion.Euler(0, 180 - arrowHeadAngle, 0) *
                   Vector3.forward;

        Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
        Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
    }
}