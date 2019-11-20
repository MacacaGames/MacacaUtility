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
}
