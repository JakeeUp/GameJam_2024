using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPathAnimator : MonoBehaviour
{
    public Camera[] cameras;
    public Camera mainCamera; 
    public float switchInterval = 5.0f;

    private int currentCameraIndex;
    private float nextSwitchTime;

    private void Start()
    {
        foreach (var cam in cameras)
        {
            cam.gameObject.SetActive(false);
        }
        mainCamera.gameObject.SetActive(false);

        if (cameras.Length > 0)
        {
            cameras[0].gameObject.SetActive(true);
            currentCameraIndex = 0;
            nextSwitchTime = Time.time + switchInterval;
        }
    }

    private void Update()
    {
        if (Time.time >= nextSwitchTime)
        {
            NextCamera();
            nextSwitchTime = Time.time + switchInterval;
        }
    }

    private void NextCamera()
    {
        cameras[currentCameraIndex].gameObject.SetActive(false);

        if (currentCameraIndex >= cameras.Length - 1)
        {
            mainCamera.gameObject.SetActive(true); // Switch back to the main camera
            if (GameManager.instance != null)
            {
                GameManager.instance.StartCountdown(); // Start the countdown
            }
        }
        else
        {
            currentCameraIndex++;
            cameras[currentCameraIndex].gameObject.SetActive(true);
        }
    }
}
