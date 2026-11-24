using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] private float reloadDelay = 0.25f;

        void Awake()
    {
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        ScoreBoardManager.ResetScore();
        SceneManager.LoadScene(0);
    }
    
    public void TriggerReloadLevel()
    {
        Invoke(nameof(ReloadLevel), reloadDelay);
    }

    private void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    public void LoadNextLevel()
    {
        ScoreBoardManager scoreBoard = FindAnyObjectByType<ScoreBoardManager>();
        if (scoreBoard != null)
        {
            scoreBoard.SaveLevelScore();
        }

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;
        if (nextScene < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
