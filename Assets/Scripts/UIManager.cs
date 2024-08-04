using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private void Start()
    {
        UpdateUI();
    }

    public void Update()
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
        }
    }
}
