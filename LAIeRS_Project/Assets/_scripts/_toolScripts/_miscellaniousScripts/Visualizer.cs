using System.Collections.Generic;

using UnityEngine;

public static class Visualizer
{
    public static void DrawLine(Vector2 startPos, Vector2 targetPos, Color color, float duration = 1)
    {
        Debug.DrawLine(startPos, targetPos, color, duration);
    }
    
    public static void DrawCircle(float radius, Vector2 posToDrawAt, Color color, float duration = 0.1f, float sharpness = 50)
    {
        radius = Mathf.Abs(radius);
        sharpness = Mathf.Abs(sharpness);
        
        sharpness = (2 * Mathf.PI) / sharpness;
        
        Vector2 lastPos = new Vector2(Mathf.Cos(0) * radius, Mathf.Sin(0) * radius) + posToDrawAt;

        float counter = 0;

        for (int i = 0; i < sharpness; i++)
        {
            counter += sharpness;

            float sinValue = Mathf.Sin(counter) * radius;
            float cosValue = Mathf.Cos(counter) * radius;

            var nextPos = new Vector2(cosValue, sinValue) + posToDrawAt;
            
            DrawLine(lastPos, nextPos, color, duration);

            lastPos = nextPos;
        }
    }

    public static void DrawPath(IList<Vector2> path, Color color, float duration)
    {
        for (int i = 0, j = 1; i < path.Count - 1; i++, j++)
        {
            Vector2 currentWayPoint = path[i];
            Vector2 targetWayPoint = path[j];
            
            DrawLine(currentWayPoint, targetWayPoint, color, duration);
        }
    }
}
