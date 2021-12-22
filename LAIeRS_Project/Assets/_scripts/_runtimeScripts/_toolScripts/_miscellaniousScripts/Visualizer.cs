using System.Collections.Generic;

using UnityEngine;

public static class Visualizer
{
    // TODO: Replace Vector2 by Vector3
    public static void DrawLine(Vector2 originPos, Vector2 targetPos, float duration = 0)
    {
        Color color = Color.green;
        Debug.DrawLine(originPos, targetPos, color, duration);
    }
    public static void DrawLine(Vector2 originPos, Vector2 targetPos, Color color, float duration = 0)
    {
        Debug.DrawLine(originPos, targetPos, color, duration);
    }
    
    public static void DrawCircle(Vector2 originPos, float radius = 1, float duration = 0)
    {
        Color color = Color.green;
        
        DrawCircle(originPos, radius, color, duration);
    }
    public static void DrawCircle(Vector2 originPos, float radius, Color color, float duration = 0)
    {
        float sharpness = 50;
        
        radius = Mathf.Abs(radius);
        sharpness = Mathf.Abs(sharpness);
        
        float iterations = sharpness;
        
        sharpness = (2 * Mathf.PI) / sharpness;

        Vector2 lastPos = new Vector2(Mathf.Cos(0) * radius, Mathf.Sin(0) * radius) + originPos;

        float counter = 0;

        for (int i = 0; i < iterations; i++)
        {
            counter += sharpness;

            float sinValue = Mathf.Sin(counter) * radius;
            float cosValue = Mathf.Cos(counter) * radius;

            var nextPos = new Vector2(cosValue, sinValue) + originPos;
            
            DrawLine(lastPos, nextPos, color, duration);

            lastPos = nextPos;
        }
    }
    
    public static void DrawSquareAt(Vector2 originPos, float size = 1, float duration = 0)
    {
        Color color = Color.green;

        DrawSquareAt(originPos, size, color, duration);
    }
    public static void DrawSquareAt(Vector2 originPos, float size, Color color, float duration = 0)
    {
        Vector2 topLeftCorner = originPos + new Vector2(0, size);
        Vector2 bottomRightCorner = originPos + new Vector2(size, 0);
        Vector2 topRightCorner = originPos + new Vector2(size, size);
        
        DrawLine(originPos, topLeftCorner, color, duration);
        DrawLine(originPos, bottomRightCorner, color, duration);
        DrawLine(topLeftCorner, topRightCorner, color, duration);
        DrawLine(bottomRightCorner, topRightCorner, color, duration);
    }

    public static void DrawRectangleAt(Vector2 originPos, Vector2 size, float duration = 0)
    {
        Color color = Color.green;
        DrawRectangleAt(originPos, size, color, duration);
    }
    public static void DrawRectangleAt(Vector2 originPos, Vector2 size, Color color, float duration = 0)
    {
        Vector2 topLeftCorner = originPos + new Vector2(0, size.y);
        Vector2 bottomRightCorner = originPos + new Vector2(size.x, 0);
        Vector2 topRightCorner = originPos + size;
        
        DrawLine(originPos, topLeftCorner, color, duration);
        DrawLine(originPos, bottomRightCorner, color, duration);
        DrawLine(topLeftCorner, topRightCorner, color, duration);
        DrawLine(bottomRightCorner, topRightCorner, color, duration);
    }
    
    public static void DrawPath(IList<Vector2> path, float duration = 0)
    {
        Color color = Color.green;
        
        DrawPath(path, color, duration);
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
