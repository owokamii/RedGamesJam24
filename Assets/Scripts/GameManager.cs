using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text scoreText;

    public int currentLevel = 1;

    private int coin;
    private int score;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void AddMoney(int amount)
    {
        coin += amount;
        coinText.text = coin.ToString();
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = score.ToString();
    }
}
