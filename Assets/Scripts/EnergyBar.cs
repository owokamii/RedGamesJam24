using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class PersistentEnergyBar : MonoBehaviour
{
    public string textMeshProButtonName = "Text";
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

        FindTextMeshPro();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindTextMeshPro();
    }

    private void FindTextMeshPro()
    {
        GameObject textMeshProButtonObject = GameObject.Find(textMeshProButtonName);

        if (textMeshProButtonObject != null)
        {
            textPersian = textMeshProButtonObject.GetComponent<TextMeshProUGUI>();
            UpdateTextMeshPro();
        }
        else
        {
            Debug.LogWarning("TextMeshPro GameObject not found by name in the current scene");
        }
    }

    public void ReduceEnergy()
    {
        Debug.Log("Reducing Energy");
        currentEnergy -= energyDrain;
        if (currentEnergy < 0) currentEnergy = 0;

        UpdateTextMeshPro();

        PlayerPrefs.SetFloat("CurrentEnergy", currentEnergy);
        PlayerPrefs.SetString("LastSaveTime", DateTime.Now.ToString());
    }

    private void RegenerateEnergy()
    {
        Debug.Log("Regenerating Energy");
        currentEnergy += energyRegenRate;
        if (currentEnergy > 1) currentEnergy = 1;

        UpdateTextMeshPro();

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

    private void UpdateTextMeshPro()
    {
        if (textPersian != null)
        {
            textPersian.text = $"Energy: {Mathf.RoundToInt(currentEnergy * 100)}%";
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("CurrentEnergy", currentEnergy);
        PlayerPrefs.SetString("LastSaveTime", DateTime.Now.ToString());
    }
}
