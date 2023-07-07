using System;
using System.Collections.Generic;
using UnityEngine;

namespace Magic
{
    public static class MathM
    {
        public static float Max(float x, float y) => Math.Max(x, y);
        public static float Max(float x, float y, float z) => Math.Max(x, Math.Max(y, z));
        public static float Max(float x, float y, float z, float w) => Math.Max(x, Math.Max(y, Math.Max(z, w)));

        public static float Mid(float x, float y) => (x + y) / 2;

        public static float Difference(float x, float y) {
            return Math.Max(x, y) - Math.Min(x, y);
        }

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

        // example: rotation = eulerangle.y
        // https://answers.unity.com/questions/54495/how-do-i-convert-angle-to-vector3.html
        public static Vector3 ConvertToVector3(float rotation) {
            return new Vector3(Mathf.Sin(Mathf.Deg2Rad * rotation), 0, Mathf.Cos(Mathf.Deg2Rad * rotation));
        }


        public static Vector3[] PositionsInDevidedLine(Vector3 startPos, Vector3 endPos, float segmentLength) {
            float distance = Vector3.Distance(startPos, endPos);
            int segmentCount = Mathf.RoundToInt(distance / segmentLength);
            Vector3[] points = new Vector3[segmentCount];

            float segmentLenghtX = (endPos.x - startPos.x) / segmentCount;
            float segmentLenghtZ = (endPos.z - startPos.z) / segmentCount;

            for (int i = 0; i < segmentCount; i++) {
                float x = startPos.x + segmentLenghtX * (i + 1);
                float z = startPos.z + segmentLenghtZ * (i + 1);
                points[i] = new Vector3(x, 0, z);
            }
            return points;
        }
    }
}