using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAndBounce : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 180f; 
    [SerializeField] private float bounceHeight = 0.5f; 
    [SerializeField] private float bounceSpeed = 2f; 

    private Vector3 startPosition;
    private float timeOffset;

    private void Start()
    {
        startPosition = transform.position;
        timeOffset = Random.Range(0f, 2f * Mathf.PI); 
    }

    private void Update()
    {
        Spin();
        Bounce();
    }

    private void Spin()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);
    }

    private void Bounce()
    {
        float newY = startPosition.y + Mathf.Sin((Time.time + timeOffset) * bounceSpeed) * bounceHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
