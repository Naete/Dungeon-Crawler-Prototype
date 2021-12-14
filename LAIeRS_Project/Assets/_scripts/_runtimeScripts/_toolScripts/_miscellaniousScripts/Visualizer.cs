using System.Collections.Generic;

using UnityEngine;

public static class Visualizer
{
    public static void DrawLineAt(Vector2 startPos, Vector2 targetPos)
    {
        Debug.DrawLine(startPos, targetPos);
    }
    public static void DrawLineAt(Vector2 startPos, Vector2 targetPos, Color color)
    {
        Debug.DrawLine(startPos, targetPos, color);
    }
    public static void DrawLineAt(Vector2 startPos, Vector2 targetPos, Color color, float duration)
    {
        Debug.DrawLine(startPos, targetPos, color, duration);
    }
    
    public static void DrawCircleAt(Vector2 origin)
    {
        float radius = 1;
        DrawCircleAt(origin, radius);
    }
    public static void DrawCircleAt(Vector2 origin, float radius)
    {
        Color color = Color.green;
        DrawCircleAt(origin, radius, color);
    }
    public static void DrawCircleAt(Vector2 origin, float radius, Color color)
    {
        float duration = 0.0f;
        DrawCircleAt(origin, radius, color, duration);
    }
    public static void DrawCircleAt(Vector2 origin, float radius, Color color, float duration)
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
            
            DrawLineAt(lastPos, nextPos, color, duration);

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
        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector2 currentWayPoint = path[i];
            Vector2 targetWayPoint = path[i+1];
            
            DrawLineAt(currentWayPoint, targetWayPoint, color, duration);
        }
    }
}
