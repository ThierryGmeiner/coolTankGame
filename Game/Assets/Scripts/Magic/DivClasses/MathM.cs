using System;
using UnityEngine;

namespace Magic
{
    public static class MathM
    {
        public static float Max(float x, float y) => Math.Max(x, y);
        public static float Max(float x, float y, float z) => Math.Max(x, Math.Max(y, z));
        public static float Max(float x, float y, float z, float w) => Math.Max(x, Math.Max(y, Math.Max(z, w)));

        public static Vector2 ClosestPointOfCircle(Vector2 pos, Vector2 center, float radius) {
            Vector2 direction = pos - center;
            direction.Normalize();
            return center + direction * radius;
        }

        public static Vector3 ClosestPointOfCircle(Vector3 pos, Vector3 center, float radius) {
            Vector3 direction = pos - center;
            direction.Normalize();
            return center + direction * radius;
        }
    }
}