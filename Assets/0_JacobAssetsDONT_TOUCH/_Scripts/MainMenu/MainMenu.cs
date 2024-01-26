using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "";
    UIManager manager;

    private void Awake()
    {
        manager = GetComponent<UIManager>();
        Debug.Log("got the manager");
        // Assuming your UIManager sets up the initial state correctly
    }
    private void Start()
    {
        UIManager.instance.fadeFromBlack = true;

    }
    public void StartGame()
    {
        UIManager.instance.fadeToBlack = true;
        StartCoroutine(StartGameSequence());
    }

    private IEnumerator StartGameSequence()
    {
        // Start fading to black
        

       

        // Then wait for an additional 2 seconds
        yield return new WaitForSeconds(2);

        // Now load the scene
        SceneManager.LoadScene(sceneToLoad);
    }

    public void QuitGame()
    {
        Debug.Log("Quit game requested");
        Application.Quit();
    }
}
