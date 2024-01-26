using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraPathAnimator : MonoBehaviour
{
    public Camera[] cameras;
    public Camera mainCamera;
    public float switchInterval = 5.0f;
    public TextMeshProUGUI uiText; // Reference to the UI Text that should appear

    private int currentCameraIndex;
    private float nextSwitchTime;
    private bool canSkip = true; // Flag to control skipping ability

    private GameManager gameManager; // Reference to the GameManager

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
        if (uiText != null)
            uiText.gameObject.SetActive(true);
        // Find the GameManager in the scene
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (Time.time >= nextSwitchTime || (canSkip && Input.GetMouseButtonDown(0)))
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
            mainCamera.gameObject.SetActive(true);
            canSkip = false;

            // Show the UI text when the main camera is activated
            // Hide the UI text at the start
            if (uiText != null)
                uiText.gameObject.SetActive(false);

            // Notify the GameManager that the camera sequence is done
            if (gameManager != null)
            {
                // Start the countdown timer in the GameManager
                gameManager.StartCountdown();
            }
        }
        else
        {
            currentCameraIndex++;
            cameras[currentCameraIndex].gameObject.SetActive(true);
        }
    }
}
