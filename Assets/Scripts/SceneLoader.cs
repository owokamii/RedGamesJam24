using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Button buttonToHide;
    public GameObject Panel;
    public GameObject ButtonClose;

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

    public void HideButtonAndActivateObjects()
    {
        if (buttonToHide != null)
        {
            buttonToHide.gameObject.SetActive(false);
        }

        if (Panel != null)
        {
            Panel.SetActive(true);
        }
    }

    public void closePanel()
    {
        if (ButtonClose != null)
        {
            ButtonClose.SetActive(false);
        }
    }
}
