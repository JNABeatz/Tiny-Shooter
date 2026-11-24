using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VictoryUI : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private Button restartButton;

    void Awake()
    {
        if (victoryPanel != null) victoryPanel.SetActive(false);

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(() =>
            {
                LevelController lc = FindAnyObjectByType<LevelController>();
                if (lc != null) lc.RestartGame();
            });
        }
    }

    public void ShowVictory()
    {
        ScoreBoardManager scoreBoard = FindAnyObjectByType<ScoreBoardManager>();
        int total = scoreBoard != null ? scoreBoard.TotalScore : 0;

        if (finalScoreText != null)
        {
            finalScoreText.text = "Puntuación obtenida: " + total;
        }

        if (victoryPanel != null) victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}