using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "";
    [SerializeField] private string sceneToLoadrules = "";
    [SerializeField] private string GOBACK = "";
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
    public void StartRules()
    {
        UIManager.instance.fadeToBlack = true;
        StartCoroutine(RulesGame());

    }
    public void GoBack()
    {

    }
    private IEnumerator StartGameSequence()
    {
        // Start fading to black
        

       

        // Then wait for an additional 2 seconds
        yield return new WaitForSeconds(2);

        // Now load the scene
        SceneManager.LoadScene(sceneToLoad);
    }
    private IEnumerator RulesGame()
    {
        // Start fading to black




        // Then wait for an additional 2 seconds
        yield return new WaitForSeconds(.5f);

        // Now load the scene
        SceneManager.LoadScene(sceneToLoadrules);
    }
    private IEnumerator GOBack()
    {
        // Start fading to black




        // Then wait for an additional 2 seconds
        yield return new WaitForSeconds(.5f);

        // Now load the scene
        SceneManager.LoadScene(GOBACK);
    }
    public void QuitGame()
    {
        Debug.Log("Quit game requested");
        Application.Quit();
    }
}
