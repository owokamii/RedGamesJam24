using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    private EnergyBar energyBar;
    private const float energyDrain = 0.3f;

    private void Start()
    {
        energyBar = FindObjectOfType<EnergyBar>();
        //CheckEnergyAndUpdateButton();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        energyBar = FindObjectOfType<EnergyBar>();
        //CheckEnergyAndUpdateButton();
    }

    public void LoadScene(string name)
    {
        Debug.Log(name);
        SceneManager.LoadSceneAsync(name);
    }

    public void LoadLevel(string name)
    {
    }

    public void SetLevelNumber(int levelNumber)
    {
        Debug.Log("Setting level number to: " + levelNumber);
        GameManager.Instance.SetCurrentLevel(levelNumber);
    }

    public void OnButtonPress(string name)
    {
        if (energyBar != null && energyBar.GetCurrentEnergy() >= energyDrain)
        {
            energyBar.ReduceEnergy();
            LoadScene(name);
        }
        else
        {
            Debug.Log("Not enough energy to load the scene");
        }
    }
}
