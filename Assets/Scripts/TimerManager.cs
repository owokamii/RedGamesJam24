using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    public float countdownTime = 60f;
    public TextMeshProUGUI countdownText;

    private float currentTime;
    private bool timerRunning = true;

    private void Start()
    {
        currentTime = countdownTime;
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

        SceneManager.LoadScene("Home");

        Time.timeScale = 1;
    }

    private void DisplayTime(float timeToDisplay)
    {
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ResumeGame()
    {
        // 恢复游戏中的活动
        Time.timeScale = 1;
        timerRunning = true;
        currentTime = countdownTime;
    }
}
