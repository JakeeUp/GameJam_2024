using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private TMP_Text[] letterTexts;
    [SerializeField] private string currentPassword = "";
    [SerializeField] private string targetPassword = "PASSWORD";
    [SerializeField] private int minutes = 1;
    [SerializeField] private int seconds = 0;
    [SerializeField] private string sceneToLoad = " ";
    [SerializeField] private TMP_Text timerText;

    private float countdownTimer;
    private bool isCountdownRunning = false;

    // Reference to the SceneTransitionManager

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        countdownTimer = minutes * 60 + seconds;

        UIManager.instance.fadeFromBlack = true;

    }

    void Start()
    {
    }

   
    public void StartCountdown()
    {
        if (!isCountdownRunning)
        {
            StartCoroutine(Countdown());
            isCountdownRunning = true;
        }
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

        isCountdownRunning = false;

        // Start the ending transition before loading a new scene

        SceneManager.LoadScene(sceneToLoad);
    }

    


   

    public void CollectLetter(char letter)
    {
        currentPassword += letter; 
        UpdateUI();
    }

    void UpdateUI()
    {
        for (int i = 0; i < currentPassword.Length; i++)
        {
            if (i < letterTexts.Length)
            {
                letterTexts[i].text = currentPassword[i].ToString();
            }
        }

        
        if (currentPassword.Equals(targetPassword))
        {
            Debug.Log("Password Complete!");
            
        }
    }
}
