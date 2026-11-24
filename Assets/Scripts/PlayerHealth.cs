using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxLives = 3;
    [SerializeField] private float fallThresholdY = -8f;

    private int currentLives;
    private bool isDead;

    void Start()
    {
        currentLives = maxLives;
        UpdateUI();
    }

    void Update()
    {
        if (!isDead && transform.position.y < fallThresholdY)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentLives = Mathf.Max(0, currentLives - damage);
        UpdateUI();

        if (currentLives <= 0)
        {
            Die();
        }
    }

    private void UpdateUI()
    {
        PlayerLivesUI livesUI = FindAnyObjectByType<PlayerLivesUI>();
        if (livesUI != null)
        {
            livesUI.UpdateLivesBar(currentLives);
        }
    }

    private void Die()
    {
        isDead = true;
        LevelController levelController = FindAnyObjectByType<LevelController>();
        if (levelController != null)
        {
            levelController.TriggerReloadLevel();
        }
    }
}
