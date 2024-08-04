using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private RotateObject ro1;
    [SerializeField] private RotateObject ro2;

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
        Time.timeScale = 1.0f;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitButton()
    {
        SceneManager.LoadSceneAsync("Home");
    }

    public void NextLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void DisableScript()
    {
        ro1.enabled = false;
        ro2.enabled = false;
    }
}
