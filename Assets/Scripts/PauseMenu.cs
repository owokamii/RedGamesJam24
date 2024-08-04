using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void PauseButton()
    {
        Time.timeScale = 0.0f;
    }

    public void ResumeButton()
    {
        Time.timeScale = 1.0f;
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
