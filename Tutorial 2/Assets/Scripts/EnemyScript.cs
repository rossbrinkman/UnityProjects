using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Vector2 startMarker;
    public Vector2 endMarker;

    public float speed = 10.0F;
    private float startTime;
    private float distance;

    void Start()
    {
        // Keep a note of the time the movement started.
        startTime = Time.time;
        distance = Vector3.Distance(startMarker, endMarker);
    }

    // Follows the target position like with a spring
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime);
        float distanceCovered = (Time.time - startTime) * speed;
        float howFar = distanceCovered / distance;

        transform.position = Vector3.Lerp(startMarker, endMarker, Mathf.PingPong(howFar, 1));
    }
}
