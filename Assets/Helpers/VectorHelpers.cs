using UnityEngine;

namespace Helpers;

public static class VectorHelpers
{
    public static Vector2 Abs(this Vector2 vec2)
        => new(Mathf.Abs(vec2.x), Mathf.Abs(vec2.y));

    public static Vector3 Abs(this Vector3 vec3)
        => new(Mathf.Abs(vec3.x), Mathf.Abs(vec3.y), Mathf.Abs(vec3.z));

    public static Vector3 XZ(this Vector2 vec2)
        => new(vec2.x, 0, vec2.y);

    public static Vector2 XZ(this Vector3 vec3)
        => new(vec3.x, vec3.z);

    public static float AbsMin(this float a, float b)
        => Mathf.Sign(a) * Mathf.Min(Mathf.Abs(a), Mathf.Abs(b));

    public static float AbsMax(this float a, float b)
        => Mathf.Sign(a) * Mathf.Max(Mathf.Abs(a), Mathf.Abs(b));
}
