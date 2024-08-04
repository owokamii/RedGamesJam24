using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

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
            SkipTutorial(); // go to level 1
        }
        else
        {
            ShowTutorial(); // go to pre-level 1
        }
    }

    private void SkipTutorial()
    {
        SceneManager.LoadScene("Home");
    }

    private void ShowTutorial()
    {
        SceneManager.LoadScene("Level1.5");
    }
}
