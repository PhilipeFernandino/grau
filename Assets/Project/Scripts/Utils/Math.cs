using System.Collections;
using UnityEngine;

namespace Utils
{
    public static class Math 
    {
        public static float Remap(float value, float x1, float x2, float y1, float y2)
        {
            return y1 + (value - x1) * (y2 - y1) / (x2 - x1);
        }

        public static bool Near(float a, float b, float epsilon = 0.1f)
        {
            return Mathf.Abs(b - a) < epsilon;
        }
    }
}