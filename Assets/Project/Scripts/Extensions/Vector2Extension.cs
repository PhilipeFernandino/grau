using UnityEngine;

public static class Vector2Extension
{
    /// <returns>
    /// <see cref="Vector3"/> (<paramref name="vector"/>.x, <paramref name="thirdValue"/>, <paramref name="vector"/>.y)
    /// </returns>
    public static Vector3 XZ(this Vector2 vector, float thirdValue = 0f)
    {
        return new Vector3(vector.x, thirdValue, vector.y);
    }

    /// <returns>
    /// <see cref="Vector3"/> (<paramref name="vector"/>.x, <paramref name="vector"/>.y, <paramref name="thirdValue"/>)
    /// </returns>
    public static Vector3 XY(this Vector2 vector, float thirdValue = 0f)
    {
        return new Vector3(vector.x, vector.y, thirdValue);
    }

    /// <returns>
    /// <see cref="Vector3"/> (<paramref name="thirdValue"/>, <paramref name="vector"/>.x, <paramref name="vector"/>.y)
    /// </returns>
    public static Vector3 YZ(this Vector2 vector, float thirdValue = 0f)
    {
        return new Vector3(thirdValue, vector.x, vector.y);
    }

    /// <returns>
    /// <see cref="Vector2"/> (<paramref name="vector"/>.y, <paramref name="vector"/>.x)
    /// </returns>
    public static Vector2 YX(this Vector2 vector)
    {
        return new Vector2(vector.y, vector.x);
    }
}