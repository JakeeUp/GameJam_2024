using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int minutes = 1; 
    [SerializeField] private int seconds = 0;
    [SerializeField] private string sceneToLoad = "  "; 
    [SerializeField] private TMP_Text timerText; 

    private float countdownTimer;

    void Start()
    {
        countdownTimer = minutes * 60 + seconds; 
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        while (countdownTimer > 0)
        {
            yield return new WaitForSeconds(1f);
            countdownTimer--;

            
            int minutesLeft = Mathf.FloorToInt(countdownTimer / 60);
            int secondsLeft = Mathf.FloorToInt(countdownTimer % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutesLeft, secondsLeft);
        }

        
        SceneManager.LoadScene(sceneToLoad);
    }
}
