using UnityEngine;

public static partial class Vector2Extension
{
    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    public static Vector2 RandomInSqaure
    {
        get
        {
            return new Vector2(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f));
        }
    }

    public static Vector2 RandomOnCircle
    {
        get
        {
            float angle = Random.Range(0f, 2f * Mathf.PI);
            return new Vector2(
                Mathf.Cos(angle),
                Mathf.Sin(angle));
        }
    }

    public static Vector2 FromVector3(Vector3 v, CovertMethod method = CovertMethod.xy)
    {
        Vector2 result = Vector2.zero;

        switch (method)
        {
            case CovertMethod.xy:
                result = new Vector2(v.x, v.y);
                break;
            case CovertMethod.xz:
                result = new Vector2(v.x, v.z);
                break;
            case CovertMethod.yz:
                result = new Vector2(v.y, v.z);
                break;
        }

        return result;
    }

    public enum CovertMethod { xy, xz, yz }

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
