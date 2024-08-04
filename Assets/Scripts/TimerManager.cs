using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public float countdownTime = 40f;
    public TextMeshProUGUI countdownText;
    public Button actionButton;
    public string sceneToLoad;

    private float currentTime;
    private bool timerRunning = true;

    private void Start()
    {
        currentTime = countdownTime;
        actionButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (timerRunning)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                timerRunning = false;
                currentTime = 0;
                OnTimerEnd();
            }
        }

        DisplayTime(currentTime);
    }

    private void OnTimerEnd()
    {
        Time.timeScale = 0;
        actionButton.gameObject.SetActive(true);
    }

    private void DisplayTime(float timeToDisplay)
    {
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        timerRunning = true;
        currentTime = countdownTime;
        actionButton.gameObject.SetActive(false);
        SceneManager.LoadScene("Home");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Home")
        {
            ResetTimer();
        }
    }

    private void ResetTimer()
    {
        currentTime = countdownTime;
        timerRunning = true;
        actionButton.gameObject.SetActive(false);
    }
}
