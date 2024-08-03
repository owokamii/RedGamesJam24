using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Button buttonToHide;
    public GameObject Level1;
    public GameObject Level2;
    public GameObject Level3;

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

        if (object1 != null)
        {
            object1.SetActive(true);
        }

        if (object2 != null)
        {
            object2.SetActive(true);
        }

        if (object3 != null)
        {
            object3.SetActive(true);
        }
    }
}
