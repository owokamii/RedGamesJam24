using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class HeartManager : MonoBehaviour
{
    public int maxHearts = 3;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public List<SpriteRenderer> heartObjects;

    private int currentHearts;
    private float nextRegenTime;
    private const string HeartsKey = "CurrentHearts";
    private const string RegenTimeKey = "NextRegenTime";
    private bool isHeartObjectsInitialized = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadHeartData();
        StartCoroutine(HeartRegenRoutine());
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void UseHeart()
    {
        if (currentHearts > 0)
        {
            currentHearts--;
            nextRegenTime = Time.time + 70f;
            SaveHeartData();
            UpdateHeartsUI();
        }
        else
        {
            Debug.Log("No hearts left!");
        }
    }

    private IEnumerator HeartRegenRoutine()
    {
        while (true)
        {
            if (currentHearts < maxHearts)
            {
                float remainingTime = nextRegenTime - Time.time;
                if (remainingTime <= 0)
                {
                    currentHearts++;
                    nextRegenTime = Time.time + 10f;
                    SaveHeartData();
                    UpdateHeartsUI();
                }
                else
                {
                    Debug.Log($"Time until next heart recovery: {remainingTime:F2} seconds");
                }
            }
            yield return new WaitForSeconds(1f); // 每秒检查一次
        }
    }

    private void UpdateHeartsUI()
    {
        if (!isHeartObjectsInitialized)
            return;

        for (int i = 0; i < heartObjects.Count; i++)
        {
            if (i < currentHearts)
            {
                heartObjects[i].sprite = fullHeart;
            }
            else
            {
                heartObjects[i].sprite = emptyHeart;
            }
        }
    }

    private void OnApplicationQuit()
    {
        SaveHeartData();
    }

    private void SaveHeartData()
    {
        PlayerPrefs.SetInt(HeartsKey, currentHearts);
        PlayerPrefs.SetFloat(RegenTimeKey, nextRegenTime - Time.time + GetCurrentUnixTimestamp());
    }

    private void LoadHeartData()
    {
        if (PlayerPrefs.HasKey(HeartsKey))
        {
            currentHearts = PlayerPrefs.GetInt(HeartsKey);
            float savedRegenTime = PlayerPrefs.GetFloat(RegenTimeKey);
            nextRegenTime = savedRegenTime - GetCurrentUnixTimestamp() + Time.time;

            float elapsedTime = Time.time - nextRegenTime + 10f;

            int heartsToRegen = Mathf.FloorToInt(elapsedTime / 10f);
            currentHearts = Mathf.Min(currentHearts + heartsToRegen, maxHearts);
            nextRegenTime = Time.time + (10f - (elapsedTime % 10f));
        }
        else
        {
            currentHearts = maxHearts;
            nextRegenTime = Time.time + 10f;
        }
    }

    private float GetCurrentUnixTimestamp()
    {
        return (float)(System.DateTime.UtcNow - new System.DateTime(1970, 1, 1)).TotalSeconds;
    }

    public void OnButtonClick()
    {
        UseHeart();
        SceneManager.LoadScene("Yvonne");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Home")
        {
            InitializeHeartObjects();
        }
    }

    private void InitializeHeartObjects()
    {
        heartObjects.Clear();
        GameObject[] heartGameObjects = GameObject.FindGameObjectsWithTag("HeartObject");
        foreach (GameObject heartGameObject in heartGameObjects)
        {
            SpriteRenderer sr = heartGameObject.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                heartObjects.Add(sr);
            }
        }
        isHeartObjectsInitialized = true;
        UpdateHeartsUI();
    }
}
