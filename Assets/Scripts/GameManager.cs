using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private string coinTextObjectName = "CoinText";
    [SerializeField] private string scoreTextObjectName = "ScoreText";
    [SerializeField] private string totalCoinTextObjectName = "TotalCoin";
    [SerializeField] private string highScoreTextObjectName = "HighScoreText"; // 新增
    [SerializeField] private string totalCoinsPrefKey = "TotalCoins";
    [SerializeField] private string highScorePrefKey = "HighScore"; // 新增

    private TMP_Text coinText;
    private TMP_Text scoreText;
    private TMP_Text totalCoinText;
    private TMP_Text highScoreText; // 新增

    public int currentLevel = 1;

    private int coin;
    private int score;
    private int totalCoins;
    private int highScore; // 新增

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            totalCoins = PlayerPrefs.GetInt(totalCoinsPrefKey, 0);
            highScore = PlayerPrefs.GetInt(highScorePrefKey, 0); // 获取保存的最高分数

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
        GameObject highScoreTextObject = GameObject.Find(highScoreTextObjectName); // 新增

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

        if (highScoreTextObject != null)
        {
            highScoreText = highScoreTextObject.GetComponent<TMP_Text>();
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

        if (highScoreText != null)
        {
            highScoreText.text = highScore.ToString();
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

        Debug.Log("AddMoney: coin = " + coin + ", totalCoins = " + totalCoins);
        UpdateUIText();

        PlayerPrefs.SetInt(totalCoinsPrefKey, totalCoins);
        PlayerPrefs.Save();
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("AddScore: score = " + score); // 添加调试信息

        //if (scoreText != null)
        //{
        //    scoreText.text = score.ToString();
            UpdateUIText();
        //}
        
        CheckAndSetHighScore();
    }

    private void CheckAndSetHighScore()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(highScorePrefKey, highScore);
            PlayerPrefs.Save();

            Debug.Log("New High Score: " + highScore); // 添加调试信息
        }
    }

    public int GetTotalCoins()
    {
        return totalCoins;
    }

    public int GetHighScore() // 新增
    {
        return highScore;
    }
}
