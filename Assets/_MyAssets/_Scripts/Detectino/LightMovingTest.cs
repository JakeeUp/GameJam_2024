using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMovingTest : MonoBehaviour
{
    //[SerializeField] private Transform[] waypoints; 
    //[SerializeField] private float speed = 5f; 

    //private int currentIndex = 0; 
    //private float threshold = 0.1f; 
    //private void Update()
    //{
    //    if (waypoints.Length == 0) return;

    //    Transform targetWaypoint = waypoints[currentIndex];

    //    float step = speed * Time.deltaTime; 
    //    transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, step);

    //    if (Vector3.Distance(transform.position, targetWaypoint.position) < threshold)
    //    {
    //        currentIndex = (currentIndex + 1) % waypoints.Length;
    //    }
    //}


    [SerializeField] private float pointA = -1f; 
    [SerializeField] private float pointB = 1f; 
    [SerializeField] private float duration = 2f; 

    private float time;

    
    void Update()
    {
       
        time += Time.deltaTime;
        float lerpTime = Mathf.PingPong(time, duration) / duration;

        float xPosition = Mathf.Lerp(pointA, pointB, lerpTime);
        transform.position = new Vector3(xPosition, transform.position.y, transform.position.z);
    }
}
