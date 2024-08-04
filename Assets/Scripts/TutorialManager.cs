using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [SerializeField] private TimerManager timerManager;

    bool playedTutorial = false;

    private void Awake()
    {
        Instance = this;
    }

    public void SaveBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
        PlayerPrefs.Save();
    }
    public bool LoadBool(string key)
    {
        return PlayerPrefs.GetInt(key, 0) == 1;
    }

    public void LoadNextScene()
    {
        if(PlayerPrefs.GetInt("TutorialCompleted", 0) == 1)
        {
            // tutorial already completed, skip tutorial
            Invoke("SkipTutorial", 1.0f); // go to level 1
            Debug.Log("skip tutorial");
        }
        else
        {
            Invoke("ShowTutorial", 1.0f); // go to pre-level 1
            Debug.Log("show tutorial");
        }
    }

    private void SkipTutorial()
    {
        SceneManager.LoadScene("Home");
    }

    private void ShowTutorial()
    {             // pause the game temporarily
        SceneManager.LoadScene("Level1.5");
        Time.timeScale = 0.0f;
    }

    public void CloseTutorial()
    {
        timerManager.enabled = true;
    }
}
