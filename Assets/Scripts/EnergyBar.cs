using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class PersistentEnergyBar : MonoBehaviour
{
    public string energyBarName = "EneegyBorder";
    public string levelButtonName = "Button_Level";
    public string textMeshProButtonName = "textPersian"; 
    private Slider energyBar;
    private Button levelButton;
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

        FindEnergyBarAndButton();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindEnergyBarAndButton();
    }

    private void FindEnergyBarAndButton()
    {
        GameObject energyBarObject = GameObject.Find(energyBarName);
        GameObject levelButtonObject = GameObject.Find(levelButtonName);

        GameObject textMeshProButtonObject = GameObject.Find(textMeshProButtonName);

        if (energyBarObject != null && levelButtonObject != null)
        {
            energyBar = energyBarObject.GetComponent<Slider>();
            levelButton = levelButtonObject.GetComponent<Button>();

            if (energyBar != null && levelButton != null)
            {
                Debug.Log("Found Slider and Button by name");
                energyBar.value = currentEnergy;
                levelButton.onClick.RemoveAllListeners();
                levelButton.onClick.AddListener(OnLevelButtonClicked);


                if (levelButton.interactable)
                {
                    Debug.Log("Button is interactable");
                }
                else
                {
                    Debug.LogWarning("Button is not interactable");
                }
            }
            else
            {
                Debug.LogWarning("Slider or Button component not found on the specified objects");
            }
        }
        else
        {
            Debug.LogWarning("Slider or Button GameObject not found by name in the current scene");
        }

        if (textMeshProButtonObject != null)
        {
            textPersian = textMeshProButtonObject.GetComponent<TextMeshProUGUI>();
            if (textPersian != null)
            {
                Debug.Log("Found TextMeshPro Button by name");
                UpdateTextMeshProButton();
            }
        }
        else
        {
            Debug.LogWarning("TextMeshPro Button GameObject not found by name in the current scene");
        }
    }

    private void OnLevelButtonClicked()
    {
        Debug.Log("Action Button Clicked");
        currentEnergy -= energyDrain;
        if (currentEnergy < 0) currentEnergy = 0;

        if (energyBar != null)
        {
            energyBar.value = currentEnergy;
        }

        if (textPersian != null)
        {
            UpdateTextMeshProButton(); // 更新TextMeshPro组件的文本
        }

        PlayerPrefs.SetFloat("CurrentEnergy", currentEnergy);
        PlayerPrefs.SetString("LastSaveTime", DateTime.Now.ToString());

        // 假设 "Yvonne" 是目标场景的名字
        SceneManager.LoadScene("Yvonne");
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
            UpdateTextMeshProButton(); // 更新TextMeshPro组件的文本
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
