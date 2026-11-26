using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxLives = 3;
    [SerializeField] private float fallThresholdY = -8f;
    [Header("Feedback visual de daño")]
    [SerializeField] private Material hitMaterial;
    [SerializeField] private float hitFlashDuration = 0.5f;

    private struct RendererBackup
    {
        public Renderer renderer;
        public Material[] originalMaterials;
    }

    private int currentLives;
    private bool isDead;
    private readonly List<RendererBackup> characterRenderers = new List<RendererBackup>();
    private Coroutine hitFlashCoroutine;

    void Awake()
    {
        foreach (Renderer rend in GetComponentsInChildren<Renderer>(true))
        {
            if (rend is ParticleSystemRenderer) continue;

            characterRenderers.Add(new RendererBackup
            {
                renderer = rend,
                originalMaterials = rend.sharedMaterials
            });
        }
    }

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

        if (hitMaterial != null)
        {
            if (hitFlashCoroutine != null)
            {
                StopCoroutine(hitFlashCoroutine);
            }
            hitFlashCoroutine = StartCoroutine(HitFlashRoutine());
        }

        if (currentLives <= 0)
        {
            Die();
        }
    }

    private IEnumerator HitFlashRoutine()
    {
        SetHitMaterial(true);
        yield return new WaitForSeconds(hitFlashDuration);
        SetHitMaterial(false);
        hitFlashCoroutine = null;
    }

    private void SetHitMaterial(bool active)
    {
        foreach (RendererBackup entry in characterRenderers)
        {
            if (entry.renderer == null) continue;

            if (active)
            {
                Material[] hitMats = new Material[entry.originalMaterials.Length];
                for (int i = 0; i < hitMats.Length; i++)
                {
                    hitMats[i] = hitMaterial;
                }
                entry.renderer.sharedMaterials = hitMats;
            }
            else
            {
                entry.renderer.sharedMaterials = entry.originalMaterials;
            }
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
