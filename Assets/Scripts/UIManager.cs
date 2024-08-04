using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private Image[] stars;
    [SerializeField] private int[] targetScores = { 100, 200, 300 };

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        if (scoreText != null && coinText != null && highScoreText != null)
        {
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        if (GameManager.Instance != null)
        {
            if (scoreText != null)
            {
                scoreText.text = "Score: " + GameManager.Instance.GetScore().ToString();
            }

            if (coinText != null)
            {
                coinText.text = "Coins: " + GameManager.Instance.GetCoins().ToString();
            }

            if (highScoreText != null)
            {
                highScoreText.text = "High Score: " + GameManager.Instance.GetHighScore().ToString();
            }

            UpdateStars(GameManager.Instance.GetScore());
        }
    }

    private void UpdateStars(int score)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].gameObject.SetActive(false);
        }

        for (int i = targetScores.Length - 1; i >= 0; i--)
        {
            if (score >= targetScores[i])
            {
                stars[i].gameObject.SetActive(true);
                break;
            }
        }
    }
}
