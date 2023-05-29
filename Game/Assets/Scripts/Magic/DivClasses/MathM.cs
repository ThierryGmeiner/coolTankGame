using System;
using UnityEngine;

namespace Magic
{
    public static class MathM
    {
        public static float Max(float x, float y) => Math.Max(x, y);
        public static float Max(float x, float y, float z) => Math.Max(x, Math.Max(y, z));
        public static float Max(float x, float y, float z, float w) => Math.Max(x, Math.Max(y, Math.Max(z, w)));
    }
}