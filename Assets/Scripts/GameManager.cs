using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private string coinTextObjectName = "CoinText";
    [SerializeField] private string scoreTextObjectName = "ScoreText";
    [SerializeField] private string totalCoinTextObjectName = "TotalCoin"; // 新增的TextMeshPro对象名
    [SerializeField] private string totalCoinsPrefKey = "TotalCoins";

    private TMP_Text coinText;
    private TMP_Text scoreText;
    private TMP_Text totalCoinText; // 用于显示总金币数的TextMeshPro对象

    public int currentLevel = 1;

    private int coin;
    private int score;
    private int totalCoins;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            totalCoins = PlayerPrefs.GetInt(totalCoinsPrefKey, 0);

            FindUIElements();
            UpdateUIText();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1;

        FindUIElements();
        UpdateUIText();
    }

    private void FindUIElements()
    {
        GameObject coinTextObject = GameObject.Find(coinTextObjectName);
        GameObject scoreTextObject = GameObject.Find(scoreTextObjectName);
        GameObject totalCoinTextObject = GameObject.Find(totalCoinTextObjectName);

        if (coinTextObject != null)
        {
            coinText = coinTextObject.GetComponent<TMP_Text>();
        }

        if (scoreTextObject != null)
        {
            scoreText = scoreTextObject.GetComponent<TMP_Text>();
        }

        if (totalCoinTextObject != null)
        {
            totalCoinText = totalCoinTextObject.GetComponent<TMP_Text>();
        }
    }

    private void UpdateUIText()
    {
        if (coinText != null)
        {
            coinText.text = coin.ToString();
        }

        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }

        if (totalCoinText != null)
        {
            totalCoinText.text = totalCoins.ToString();
        }
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
    }

    public void AddMoney(int amount)
    {
        coin += amount;
        totalCoins += amount;

        UpdateUIText();

        PlayerPrefs.SetInt(totalCoinsPrefKey, totalCoins);
        PlayerPrefs.Save();
    }

    public void AddScore(int amount)
    {
        score += amount;
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    public int GetTotalCoins()
    {
        return totalCoins;
    }
}
