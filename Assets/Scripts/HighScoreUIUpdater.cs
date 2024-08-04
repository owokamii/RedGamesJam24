using UnityEngine;
using TMPro;

public class HighScoreUIUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText;

    private void OnEnable()
    {
        UpdateHighScoreUI();
    }

    private void UpdateHighScoreUI()
    {
        if (GameManager.Instance != null && highScoreText != null)
        {
            highScoreText.text =  GameManager.Instance.GetHighScore().ToString();
        }
    }
}
