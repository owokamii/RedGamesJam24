using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private string coinTextObjectName = "CoinText";
    [SerializeField] private string scoreTextObjectName = "ScoreText";

    private TMP_Text coinText;
    private TMP_Text scoreText;

    public int currentLevel = 1;

    private int coin;
    private int score;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
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

        GameObject coinTextObject = GameObject.Find(coinTextObjectName);
        GameObject scoreTextObject = GameObject.Find(scoreTextObjectName);

        if (coinTextObject != null)
        {
            coinText = coinTextObject.GetComponent<TMP_Text>();
            coinText.text = coin.ToString();
        }

        if (scoreTextObject != null)
        {
            scoreText = scoreTextObject.GetComponent<TMP_Text>();
            scoreText.text = score.ToString();
        }
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void AddMoney(int amount)
    {
        coin += amount;
        if (coinText != null)
        {
            coinText.text = coin.ToString();
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }
}
