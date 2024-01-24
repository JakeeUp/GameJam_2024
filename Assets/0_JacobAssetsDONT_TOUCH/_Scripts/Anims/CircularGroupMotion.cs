using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularGroupMotion : MonoBehaviour
{

    public float rotationSpeed = 30f; // Speed of rotation around the Y-axis

    void Update()
    {
        // Rotate around the Y axis at the specified speed
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
    }
}
