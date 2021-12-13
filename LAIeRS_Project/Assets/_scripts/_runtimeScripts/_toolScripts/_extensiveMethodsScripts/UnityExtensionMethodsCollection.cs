using UnityEngine;

namespace LAIeRS.ExtensiveMethods
{
    public static class UnityExtensionMethods
    {
        public static Vector2 GetDirectionTo(this Vector2 originPos, Vector2 targetPos)
        {
            return (targetPos - originPos).normalized;
        }
        
        public static float GetDistanceTo(this Vector2 originPos, Vector2 targetPos)
        {
            return Vector2.Distance(originPos, targetPos);
        }
        
        public static Vector3 GetDirectionTo(this Vector3 originPos, Vector3 targetPos)
        {
            return (targetPos - originPos).normalized;
        }
    }
}