using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private bool isHeartObjectsInitialized = true;
    private Coroutine regenCoroutine;
    private bool IsHealing = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadHeartData();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void Update()
    {

    }

    public void UseHeart()
    {
        if (currentHearts > 0)
        {
            Debug.Log("Using heart. Current hearts before usage: " + currentHearts);
            currentHearts--;
            SaveHeartData();
            UpdateHeartsUI();
            if (currentHearts < maxHearts && !IsHealing)
            {
                StartHeartRegenRoutine();
            }
        }
        else
        {
            Debug.Log("No hearts left!");
        }
    }

    private void StartHeartRegenRoutine()
    {
        if (regenCoroutine == null)
        {
            IsHealing = true;
            nextRegenTime = Time.time + 30f;
            regenCoroutine = StartCoroutine(HeartRegenRoutine());
        }
    }

    private IEnumerator HeartRegenRoutine()
    {
        while (currentHearts < maxHearts)
        {
            float remainingTime = nextRegenTime - Time.time;
            if (remainingTime <= 0)
            {
                Debug.Log("Regenerating heart. Current hearts before regen: " + currentHearts);
                currentHearts++;
                SaveHeartData();
                UpdateHeartsUI();

                if (currentHearts < maxHearts)
                {
                    nextRegenTime = Time.time + 30f;
                }
                else
                {
                    regenCoroutine = null;
                    IsHealing = false;
                    yield break;
                }
            }
            else
            {
                yield return new WaitForSeconds(remainingTime);
            }
        }
    }

    private void UpdateHeartsUI()
    {
        //if (!isHeartObjectsInitialized)
        //    return;

        Debug.Log("Updating hearts UI. Current hearts: " + currentHearts);
        for (int i = 0; i < heartObjects.Count; i++)
        {
            if (heartObjects[i] != null)
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
    }

    private void OnApplicationQuit()
    {
        SaveHeartData();
    }

    private void SaveHeartData()
    {
        PlayerPrefs.SetInt(HeartsKey, currentHearts);
        PlayerPrefs.SetFloat(RegenTimeKey, nextRegenTime - Time.time + GetCurrentUnixTimestamp());
        Debug.Log("Saving heart data. Current hearts: " + currentHearts + ", next regen time: " + nextRegenTime);
    }

    private void LoadHeartData()
    {
        if (PlayerPrefs.HasKey(HeartsKey))
        {
            currentHearts = PlayerPrefs.GetInt(HeartsKey);
            float savedRegenTime = PlayerPrefs.GetFloat(RegenTimeKey);
            nextRegenTime = savedRegenTime - GetCurrentUnixTimestamp() + Time.time;

            float elapsedTime = GetCurrentUnixTimestamp() - savedRegenTime;

            int heartsToRegen = Mathf.FloorToInt(elapsedTime / 30f);
            currentHearts = Mathf.Min(currentHearts + heartsToRegen, maxHearts);

            if (currentHearts < maxHearts)
            {
                float remainingTimeToNextRegen = 30f - (elapsedTime % 30f);
                nextRegenTime = Time.time + remainingTimeToNextRegen;
                StartHeartRegenRoutine();
            }
            else
            {
                nextRegenTime = Time.time + 30f;
            }

            Debug.Log("Loaded heart data. Current hearts: " + currentHearts + ", next regen time: " + nextRegenTime);
            UpdateHeartsUI();
        }
        else
        {
            currentHearts = maxHearts;
            nextRegenTime = Time.time + 30f;
            Debug.Log("No saved heart data. Setting current hearts to max: " + maxHearts);
        }
    }

    private float GetCurrentUnixTimestamp()
    {
        return (float)(System.DateTime.UtcNow - new System.DateTime(1970, 1, 1)).TotalSeconds;
    }

    public void OnButtonClick()
    {
        UseHeart();
        //SceneManager.LoadScene("Yvonne");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeHeartObjects();
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
