using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class EnergyBar : MonoBehaviour
{
    public string textMeshProButtonName = "Text";
    public string targetButtonName = "Button_Level";
    private TextMeshProUGUI textMeshProButton;
    private float currentEnergy;
    private const float energyDrain = 0.5f;
    private const float energyRegenRate = 0.1f;
    private const float regenInterval = 10f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        currentEnergy = PlayerPrefs.GetFloat("CurrentEnergy", 1f);

        RestoreEnergyFromLastSession();
        FindTextMeshPro();
        UpdateTextMeshPro();
        InvokeRepeating("RegenerateEnergy", regenInterval, regenInterval);

        BindButton();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindTextMeshPro();
        UpdateTextMeshPro();
        BindButton();
    }

    private void FindTextMeshPro()
    {
        GameObject textMeshProButtonObject = GameObject.Find(textMeshProButtonName);

        if (textMeshProButtonObject != null)
        {
            textMeshProButton = textMeshProButtonObject.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogWarning("TextMeshPro GameObject not found by name in the current scene");
        }
    }

    private void BindButton()
    {
        GameObject targetButtonObject = GameObject.Find(targetButtonName);

        if (targetButtonObject != null)
        {
            Button targetButton = targetButtonObject.GetComponent<Button>();
            if (targetButton != null)
            {
                targetButton.onClick.AddListener(ReduceEnergy);
            }
            else
            {
                Debug.LogWarning("Button component not found on the specified GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("Target button GameObject not found by name in the current scene.");
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
        if (textMeshProButton != null)
        {
            textMeshProButton.text = $"Energy: {currentEnergy * 100:F2}%";
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("CurrentEnergy", currentEnergy);
        PlayerPrefs.SetString("LastSaveTime", DateTime.Now.ToString());
    }
}
