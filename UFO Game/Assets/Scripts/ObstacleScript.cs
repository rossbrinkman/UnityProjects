using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    public Transform startMarker;
    public Transform endMarker;

    public float speed = 10.0F;
    private float startTime;
    private float distance;

    void Start()
    {
        // Keep a note of the time the movement started.
        startTime = Time.time;
        distance = Vector3.Distance(startMarker.position, endMarker.position);
    }

    // Follows the target position like with a spring
    void Update()
    {
      
        float distanceCovered = (Time.time - startTime) * speed;
        float howFar = distanceCovered / distance;

        transform.position = Vector3.Lerp(startMarker.position, endMarker.position, Mathf.PingPong(howFar, 1));
    }
}
