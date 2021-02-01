using UnityEngine;

public class Utils
{
    public static Vector2 RandomPointInCircle(float radius)
    {
        var angle = Random.value * 2.0f * Mathf.PI;
        var distance = radius * Mathf.Sqrt(Random.value);
        
        return new Vector2(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle));
    }
}