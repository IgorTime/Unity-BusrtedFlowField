using UnityEngine;

public static class ArrowGizmo
{
    public static void Draw(
        Vector3 pos,
        Vector3 direction,
        Color? color = null,
        float arrowHeadLength = 0.2f,
        float arrowHeadAngle = 20.0f)
    {
        Gizmos.color = color ?? Color.white;
        Gizmos.DrawRay(pos, direction);

        if (direction == Vector3.zero) return;

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