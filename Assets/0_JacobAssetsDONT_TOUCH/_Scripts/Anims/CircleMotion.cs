using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMotion : MonoBehaviour
{
    public float radius = 5f; // Radius of the circle
    public float speed = 1f; // Speed of rotation around the circle

    private Vector3 centerPosition; // Center position for the circle
    private float angle; // Current angle of the object on the circular path

    private void Start()
    {
        // Store the starting position as the center
        centerPosition = transform.position;
    }

    private void Update()
    {
        // Increase the angle over time
        angle += speed * Time.deltaTime;

        // Calculate the new position relative to the center
        float x = centerPosition.x + Mathf.Cos(angle) * radius;
        float z = centerPosition.z + Mathf.Sin(angle) * radius;

        // Update the position to be offset from the center
        transform.position = new Vector3(x, transform.position.y, z);
    }
}
