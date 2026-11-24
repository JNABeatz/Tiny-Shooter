using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 3f;
    [SerializeField] private HealthBarController healthBar;

    private float currentHealth;
    private bool isDead;

    public bool IsDead => isDead;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null) healthBar.SetMaxValue(maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (healthBar != null) healthBar.UpdateBar(currentHealth);

        if (currentHealth <= 0f)
        {
            isDead = true;
            ScoreBoardManager scoreBoard = FindAnyObjectByType<ScoreBoardManager>();
            if (scoreBoard != null) scoreBoard.RefreshProgress();
            Destroy(gameObject);
        }
    }
}