﻿using UnityEngine;

public class Utils
{
    public static Vector2 RandomPointInCircle(float radius)
    {
        var angle = Random.value * 2.0f * Mathf.PI;
        var distance = radius * Mathf.Sqrt(Random.value);

        return new Vector2(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle));
    }

    public static Vector3 RandomPointInBox(Vector3 position, Vector3 size)
    {
        size /= 2.0f; // from center

        var x = Random.Range(position.x - size.x, position.x + size.x);
        var y = Random.Range(position.y - size.y, position.y + size.y);
        var z = Random.Range(position.z - size.z, position.z + size.z);

        return new Vector3(x, y, z);
    }

    public static Vector3 RandomPointInBox(BoxCollider boxCollider)
    {
        return RandomPointInBox(boxCollider.transform.position + boxCollider.center, boxCollider.size);
    }
}