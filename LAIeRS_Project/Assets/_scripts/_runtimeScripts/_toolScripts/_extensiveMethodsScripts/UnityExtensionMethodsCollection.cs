using UnityEngine;

namespace LAIeRS.ExtensiveMethods
{
    public static class UnityExtensionMethods
    {
        public static Vector3 GetDirectionTo(this Vector3 originPos, Vector3 targetPos)
        {
            return (targetPos - originPos).normalized;
        }
        
        public static float GetDistanceTo(this Vector3 originPos, Vector3 targetPos)
        {
            return Vector3.Distance(originPos, targetPos);
        }

        public static RaycastHit2D ShootRayTo(this Vector3 originPos, Vector3 direction, float distance)
        {
            return Physics2D.Raycast(originPos, direction, distance);
        }
        
        public static RaycastHit2D ShootRayTo(this Vector3 originPos, Vector3 direction, float distance, 
            LayerMask collideWith)
        {
            return Physics2D.Raycast(originPos, direction, distance, collideWith);
        }

        public static bool Is(this RaycastHit2D me, Transform other)
        {
            return other != null && me.transform == other;
        }
        
        public static bool IsNot(this RaycastHit2D me, Transform other)
        {
            return !me.transform == other;
        }
    }
}