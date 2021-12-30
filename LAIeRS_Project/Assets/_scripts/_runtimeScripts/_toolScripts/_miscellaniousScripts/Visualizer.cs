using System.Collections.Generic;

using UnityEngine;

public static class Visualizer
{
    // TODO: Replace Vector2 by Vector3
    public static void DrawLine(Vector2 originPosition, Vector2 targetPosition, float duration = 0)
    {
        Color color = Color.green;
        Debug.DrawLine(originPosition, targetPosition, color, duration);
    }
    public static void DrawLine(Vector2 originPosition, Vector2 targetPosition, Color color, float duration = 0)
    {
        Debug.DrawLine(originPosition, targetPosition, color, duration);
    }
    
    public static void DrawCircle(Vector2 originPosition, float radius = 1, float duration = 0)
    {
        Color color = Color.green;
        
        DrawCircle(originPosition, radius, color, duration);
    }
    public static void DrawCircle(Vector2 originPosition, float radius, Color color, float duration = 0)
    {
        float sharpness = 50;
        
        radius = Mathf.Abs(radius);
        sharpness = Mathf.Abs(sharpness);
        
        float iterations = sharpness;
        
        sharpness = (2 * Mathf.PI) / sharpness;

        Vector2 lastPosition = new Vector2(Mathf.Cos(0) * radius, Mathf.Sin(0) * radius) + originPosition;

        float counter = 0;

        for (int i = 0; i < iterations; i++)
        {
            counter += sharpness;

            float sinValue = Mathf.Sin(counter) * radius;
            float cosValue = Mathf.Cos(counter) * radius;

            var nextPosition = new Vector2(cosValue, sinValue) + originPosition;
            
            DrawLine(lastPosition, nextPosition, color, duration);

            lastPosition = nextPosition;
        }
    }
    
    public static void DrawSquareAt(Vector2 originPosition, float size = 1, float duration = 0)
    {
        Color color = Color.green;

        DrawSquareAt(originPosition, size, color, duration);
    }
    public static void DrawSquareAt(Vector2 originPosition, float size, Color color, float duration = 0)
    {
        Vector2 topLeftCornerPosition = originPosition + new Vector2(0, size);
        Vector2 bottomRightCornerPosition = originPosition + new Vector2(size, 0);
        Vector2 topRightCornerPosition = originPosition + new Vector2(size, size);
        
        DrawLine(originPosition, topLeftCornerPosition, color, duration);
        DrawLine(originPosition, bottomRightCornerPosition, color, duration);
        DrawLine(topLeftCornerPosition, topRightCornerPosition, color, duration);
        DrawLine(bottomRightCornerPosition, topRightCornerPosition, color, duration);
    }

    public static void DrawRectangleAt(Vector2 originPosition, Vector2 size, float duration = 0)
    {
        Color color = Color.green;
        DrawRectangleAt(originPosition, size, color, duration);
    }
    public static void DrawRectangleAt(Vector2 originPosition, Vector2 size, Color color, float duration = 0)
    {
        Vector2 topLeftCornerPosition = originPosition + new Vector2(0, size.y);
        Vector2 bottomRightCornerPosition = originPosition + new Vector2(size.x, 0);
        Vector2 topRightCornerPosition = originPosition + size;
        
        DrawLine(originPosition, topLeftCornerPosition, color, duration);
        DrawLine(originPosition, bottomRightCornerPosition, color, duration);
        DrawLine(topLeftCornerPosition, topRightCornerPosition, color, duration);
        DrawLine(bottomRightCornerPosition, topRightCornerPosition, color, duration);
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
