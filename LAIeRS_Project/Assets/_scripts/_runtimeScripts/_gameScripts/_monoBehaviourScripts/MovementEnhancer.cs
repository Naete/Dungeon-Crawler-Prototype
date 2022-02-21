using System;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;

public static class MovementEnhancer
{
    public static void Accelerate(this Rigidbody2D rigidbody2D, Vector2 direction, float maxSpeed, float time)
    {
        Vector2 accelerationSpeed = direction * (maxSpeed / time);
        
        rigidbody2D.velocity += accelerationSpeed * Time.fixedDeltaTime;
    }
    
    public static void Decelerate(this Rigidbody2D rigidbody2D, float lastHighestSpeed, float time)
    {
        Vector2 decelerationSpeed = rigidbody2D.velocity.normalized * (lastHighestSpeed / time);

        if (rigidbody2D.velocity.magnitude > 0)
            rigidbody2D.velocity -= decelerationSpeed * Time.fixedDeltaTime;
        else
            rigidbody2D.velocity *= Vector2.zero;
    }
    
    public static Task AccelerateAsync(this Rigidbody2D rigidbody, float duration, float targetSpeed,
        CancellationTokenSource token)
    {
        float accelerationUpSpeed = targetSpeed / duration;
        
        return RunTask(
            () => rigidbody.velocity += rigidbody.velocity.normalized * accelerationUpSpeed * Time.deltaTime,
            () => rigidbody.velocity = 
                (rigidbody.velocity.magnitude > targetSpeed) 
                    ? rigidbody.velocity.normalized * targetSpeed 
                    : rigidbody.velocity
            , duration, token);
    }
    
    public static Task DecelerateAsync(this Rigidbody2D rigidbody, float duration, CancellationTokenSource token)
    {
        float decelerationFallSpeed = rigidbody.velocity.magnitude / duration;
        
        return RunTask(
            () => rigidbody.velocity -= rigidbody.velocity.normalized * decelerationFallSpeed * Time.deltaTime,
            () => rigidbody.velocity *= (rigidbody.velocity.magnitude > 0) ? 0 : 1
            , duration, token);
    }

    public static CancellationTokenSource GetNewToken(this CancellationTokenSource tokenSource)
    {
        tokenSource?.Cancel();
        tokenSource?.Dispose();
        return new CancellationTokenSource();
    }

    public static void Reset(this CancellationTokenSource source, out CancellationTokenSource target)
    {
        source?.Cancel();
        source?.Dispose();
        
        target = new CancellationTokenSource();
    }

    private static async Task RunTask(Action action, Action action2, float duration, CancellationTokenSource token)
    {
        token.CancelAfter(TimeSpan.FromSeconds(duration));
        
        while (!token.IsCancellationRequested)
        {
            action();
            await Task.Yield();
        }

        action2();
        
        Debug.Log("RunTask done");
    }
}
