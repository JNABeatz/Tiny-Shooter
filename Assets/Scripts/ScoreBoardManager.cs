using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreBoardManager : MonoBehaviour
{
    public static int SavedTotalScore = 0;
    [SerializeField] private TMP_Text scoreBoardText;
    [SerializeField] private int requiredStones = 3;
    private int levelScore;
    private bool doorOpened;
    public int LevelScore => levelScore;
    public int TotalScore => SavedTotalScore + levelScore;
    public bool HasAllStones => levelScore >= requiredStones;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetOnEnterPlayMode()
    {
        SavedTotalScore = 0;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SavedTotalScore = 0;
        }

        levelScore = 0;
        doorOpened = false;
        if (requiredStones <= 0)
        {
            requiredStones = FindObjectsByType<MagicStone>(FindObjectsSortMode.None).Length;
        }
        RefreshProgress();
    }

    public void IncreaseScore(int amount)
    {
        levelScore += amount;
        RefreshProgress();
    }

    public void RefreshProgress()
    {
        int aliveEnemies = CountAliveEnemies();

        if (HasAllStones && aliveEnemies == 0 && !doorOpened)
        {
            Switch switchObj = FindAnyObjectByType<Switch>(FindObjectsInactive.Include);
            if (switchObj != null) switchObj.UnlockSwitch();
        }

        UpdateUI(aliveEnemies);
    }

    public void SetDoorOpened()
    {
        doorOpened = true;
        UpdateUI(CountAliveEnemies());
    }

    public void SaveLevelScore()
    {
        SavedTotalScore += levelScore;
    }

    public static void ResetScore()
    {
        SavedTotalScore = 0;
    }

    private int CountAliveEnemies()
    {
        EnemyHealth[] enemies = FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None);
        int alive = 0;
        foreach (EnemyHealth enemy in enemies)
        {
            if (!enemy.IsDead) alive++;
        }
        return alive;
    }

    private void UpdateUI(int aliveEnemies)
    {
        if (scoreBoardText == null) return;

        if (doorOpened)
        {
            scoreBoardText.text = $"¡Puerta abierta! Cruza el portal";
        }
        else if (!HasAllStones)
        {
            scoreBoardText.text = $"Recoge las piedras para destruir el portal ({levelScore}/{requiredStones})";
        }
        else if (aliveEnemies > 0)
        {
            scoreBoardText.text = $"Mata a todos los enemigos para desbloquear la llave";
        }
        else
        {
            scoreBoardText.text = $"Recoge la llave para abrir la puerta";
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
