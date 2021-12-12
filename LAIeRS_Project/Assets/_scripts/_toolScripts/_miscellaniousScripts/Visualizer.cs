using System.Collections.Generic;

using UnityEngine;

public static class Visualizer
{
    public static void DrawLine(Vector2 startPos, Vector2 targetPos)
    {
        Debug.DrawLine(startPos, targetPos);
    }
    public static void DrawLine(Vector2 startPos, Vector2 targetPos, Color color)
    {
        Debug.DrawLine(startPos, targetPos, color);
    }
    public static void DrawLine(Vector2 startPos, Vector2 targetPos, Color color, float duration)
    {
        Debug.DrawLine(startPos, targetPos, color, duration);
    }
    
    public static void DrawCircle(Vector2 origin)
    {
        float radius = 1;
        DrawCircle(origin, radius);
    }
    public static void DrawCircle(Vector2 origin, float radius)
    {
        Color color = Color.green;
        DrawCircle(origin, radius, color);
    }
    public static void DrawCircle(Vector2 origin, float radius, Color color)
    {
        float duration = 0.0f;
        DrawCircle(origin, radius, color, duration);
    }
    public static void DrawCircle(Vector2 origin, float radius, Color color, float duration)
    {
        float sharpness = 50;
        
        radius = Mathf.Abs(radius);
        sharpness = Mathf.Abs(sharpness);
        
        float iterations = sharpness;
        
        sharpness = (2 * Mathf.PI) / sharpness;

        Vector2 lastPos = new Vector2(Mathf.Cos(0) * radius, Mathf.Sin(0) * radius) + origin;

        float counter = 0;

        for (int i = 0; i < iterations; i++)
        {
            counter += sharpness;

            float sinValue = Mathf.Sin(counter) * radius;
            float cosValue = Mathf.Cos(counter) * radius;

            var nextPos = new Vector2(cosValue, sinValue) + origin;
            
            DrawLine(lastPos, nextPos, color, duration);

            lastPos = nextPos;
        }
    }

    public static void DrawPath(IList<Vector2> path)
    {
        Color color = Color.green;
        DrawPath(path, color);
    }
    public static void DrawPath(IList<Vector2> path, Color color)
    {
        float duration = 0.0f;
        DrawPath(path, color, duration);
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
