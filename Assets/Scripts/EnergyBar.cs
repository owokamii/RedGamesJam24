using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class PersistentEnergyBar : MonoBehaviour
{
    public string energyBarName = "EneegyBorder";
    public string textMeshProButtonName = "textPersian";
    private Slider energyBar;
    private TextMeshProUGUI textPersian;
    private float currentEnergy;
    private const float energyDrain = 0.5f;
    private const float energyRegenRate = 0.1f;
    private const float regenInterval = 10f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        currentEnergy = PlayerPrefs.GetFloat("CurrentEnergy", 1f);

        RestoreEnergyFromLastSession();
        InvokeRepeating("RegenerateEnergy", regenInterval, regenInterval);
        SceneManager.sceneLoaded += OnSceneLoaded;

        FindEnergyBarAndText();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindEnergyBarAndText();
    }

    private void FindEnergyBarAndText()
    {
        GameObject energyBarObject = GameObject.Find(energyBarName);
        GameObject textMeshProButtonObject = GameObject.Find(textMeshProButtonName);

        if (energyBarObject != null)
        {
            energyBar = energyBarObject.GetComponent<Slider>();
            energyBar.value = currentEnergy;
        }
        else
        {
            Debug.LogWarning("Slider GameObject not found by name in the current scene");
        }

        if (textMeshProButtonObject != null)
        {
            textPersian = textMeshProButtonObject.GetComponent<TextMeshProUGUI>();
            UpdateTextMeshProButton();
        }
        else
        {
            Debug.LogWarning("TextMeshPro Button GameObject not found by name in the current scene");
        }
    }

    public void ReduceEnergy()
    {
        Debug.Log("Reducing Energy");
        currentEnergy -= energyDrain;
        if (currentEnergy < 0) currentEnergy = 0;

        if (energyBar != null)
        {
            energyBar.value = currentEnergy;
        }

        if (textPersian != null)
        {
            UpdateTextMeshProButton();
        }

        PlayerPrefs.SetFloat("CurrentEnergy", currentEnergy);
        PlayerPrefs.SetString("LastSaveTime", DateTime.Now.ToString());
    }

    private void RegenerateEnergy()
    {
        Debug.Log("Regenerating Energy");
        currentEnergy += energyRegenRate;
        if (currentEnergy > 1) currentEnergy = 1;

        if (energyBar != null)
        {
            energyBar.value = currentEnergy;
        }

        if (textPersian != null)
        {
            UpdateTextMeshProButton();
        }

        PlayerPrefs.SetFloat("CurrentEnergy", currentEnergy);
        PlayerPrefs.SetString("LastSaveTime", DateTime.Now.ToString());
    }

    private void RestoreEnergyFromLastSession()
    {
        string lastSaveTimeStr = PlayerPrefs.GetString("LastSaveTime", DateTime.Now.ToString());
        DateTime lastSaveTime = DateTime.Parse(lastSaveTimeStr);
        TimeSpan timeSinceLastSave = DateTime.Now - lastSaveTime;

        float regenTimes = (float)(timeSinceLastSave.TotalSeconds / regenInterval);
        float totalRegen = regenTimes * energyRegenRate;

        currentEnergy += totalRegen;
        if (currentEnergy > 1) currentEnergy = 1;
    }

    private void UpdateTextMeshProButton()
    {
        if (textPersian != null)
        {
            textPersian.text = $"Energy: {currentEnergy * 100}%";
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("CurrentEnergy", currentEnergy);
        PlayerPrefs.SetString("LastSaveTime", DateTime.Now.ToString());
    }
}
