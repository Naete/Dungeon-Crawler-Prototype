using System.Collections.Generic;

using UnityEngine;

public static class Visualizer
{
    public static void DrawLine(Vector2 startPos, Vector2 targetPos)
    {
        Debug.DrawLine(startPos, targetPos);
    }
    public static void DrawLine(Vector2 startPos, Vector2 targetPos, Color color, float duration = 0)
    {
        Debug.DrawLine(startPos, targetPos, color, duration);
    }
    
    public static void DrawCircle(Vector2 origin, float radius = 1)
    {
        Color color = Color.green;
        
        DrawCircle(origin, radius, color);
    }
    public static void DrawCircle(Vector2 origin, float radius, Color color, float duration = 0)
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
    
    public static void DrawSquareAt(Vector2 origin, float size = 1)
    {
        Color color = Color.green;

        DrawSquareAt(origin, size, color);
    }
    public static void DrawSquareAt(Vector2 origin, float size, Color color, float duration = 0)
    {
        Vector2 topLeftCorner = origin + new Vector2(0, size);
        Vector2 bottomRightCorner = origin + new Vector2(size, 0);
        Vector2 topRightCorner = origin + new Vector2(size, size);
        
        DrawLine(origin, topLeftCorner, color, duration);
        DrawLine(origin, bottomRightCorner, color, duration);
        DrawLine(topLeftCorner, topRightCorner, color, duration);
        DrawLine(bottomRightCorner, topRightCorner, color, duration);
    }

    public static void DrawPath(IList<Vector2> path)
    {
        Color color = Color.green;
        
        DrawPath(path, color);
    }
    public static void DrawPath(IList<Vector2> path, Color color, float duration = 0)
    {
        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector2 currentWayPoint = path[i];
            Vector2 targetWayPoint = path[i+1];
            
            DrawLine(currentWayPoint, targetWayPoint, color, duration);
        }
    }
}
