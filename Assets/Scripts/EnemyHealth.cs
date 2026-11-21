using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 3f;
    [SerializeField] private HealthBarController healthBar;

    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null) healthBar.SetMaxValue(maxHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (healthBar != null) healthBar.UpdateBar(currentHealth);

        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }
}