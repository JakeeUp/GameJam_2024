using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public CanvasGroup fadePanel;
    public float fadeDuration = 1f;

    private void Start()
    {
        // Start with the panel fully visible (alpha = 1)
        fadePanel.alpha = 1;
        // Begin the scene with a fade-in effect
        FadeIn();
    }

    public void FadeIn()
    {
        StartCoroutine(FadeCanvasGroup(fadePanel, fadePanel.alpha, 0, fadeDuration));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCanvasGroup(fadePanel, fadePanel.alpha, 1, fadeDuration));
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(TransitionToScene(sceneName));
    }

    private IEnumerator TransitionToScene(string sceneName)
    {
        FadeOut();
        yield return new WaitForSeconds(fadeDuration);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, elapsedTime / duration);
            yield return null;
        }

        cg.alpha = end;
    }
}
