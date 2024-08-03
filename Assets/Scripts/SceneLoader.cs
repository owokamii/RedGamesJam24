using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string name)
    {
        Debug.Log(name);
        SceneManager.LoadSceneAsync(name);
    }

    public void SetLevelNumber(int levelNumber)
    {
        Debug.Log("Setting level number to: " + levelNumber);

        GameManager.Instance.SetCurrentLevel(levelNumber);
    }
}
