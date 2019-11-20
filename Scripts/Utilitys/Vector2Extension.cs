using UnityEngine;
using System.Collections;

public static partial class Vector2Extension
{
    public static float ToAngle(this Vector2 vector)
    {
        if (Mathf.Abs(vector.magnitude) <= float.Epsilon)
            return 0;

        vector.Normalize();
        float cos = vector.x / vector.magnitude;
        float angle = Mathf.Acos(cos);

        if (vector.y < 0)
            angle = 2 * Mathf.PI - angle;

        angle *= Mathf.Rad2Deg;

        return angle;
    }

    public static Vector2 FromAngle(float angle)
    {
        angle *= Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
}
