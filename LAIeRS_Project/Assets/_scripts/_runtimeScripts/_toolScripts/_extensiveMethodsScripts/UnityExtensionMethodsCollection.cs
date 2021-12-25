using UnityEngine;

namespace LAIeRS.ExtensiveMethods
{
    public static class UnityExtensionMethods
    {
        public static Vector2 GetDirectionTo(this Vector2 originPosition, Vector2 targetPosition)
        {
            return (targetPosition - originPosition).normalized;
        }
        
        public static float GetDistanceTo(this Vector2 originPosition, Vector2 targetPosition)
        {
            return Vector2.Distance(originPosition, targetPosition);
        }
        
        public static Vector3 GetDirectionTo(this Vector3 originPosition, Vector3 targetPosition)
        {
            return (targetPosition - originPosition).normalized;
        }
    }
}