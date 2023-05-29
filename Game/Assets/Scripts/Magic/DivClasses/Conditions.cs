using UnityEngine;

namespace Magic
{
    public static class Conditions
    {
        public static bool DirectPathIsBlocked(Vector3 pos, Vector3 pos2, float radius, LayerMask layer) {
            Vector3 direction = pos - pos2;
            Ray ray = new Ray(pos2, direction);
            float rayLength = Vector3.Distance(pos2, pos);
            return Physics.SphereCast(ray, radius, rayLength, layer);
        }
    }
}