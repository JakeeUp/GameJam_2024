using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ENDGANME : MonoBehaviour
{
    public string sceneToLoad = "YourSceneNameHere"; // The name of the scene you want to load

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure only the player can trigger the scene load
        {
            SceneManager.LoadScene(sceneToLoad); // Load the scene
        }
    }
}
