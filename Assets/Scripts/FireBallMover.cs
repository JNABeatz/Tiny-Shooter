using UnityEngine;

public class FireBallMover : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 2;
    [SerializeField] private string targetTag = "Enemy";
    [SerializeField] private GameObject explosionVFX;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.linearVelocity = transform.forward * speed;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return; // Ignora otros triggers
        if (other.CompareTag(targetTag))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null) enemyHealth.TakeDamage(1f);

            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null) playerHealth.TakeDamage(1);

            if (explosionVFX != null) Instantiate(explosionVFX, transform.position, Quaternion.identity);

            Destroy(gameObject);
            return;
        }

        // Si choca contra muros o suelo, se destruye
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (explosionVFX != null) Instantiate(explosionVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
