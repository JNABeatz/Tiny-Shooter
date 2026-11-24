using UnityEngine;
using System.Collections;

public class SpawnPortal : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxAliveEnemies = 4;
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }
    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            int aliveEnemies = FindObjectsByType<EnemyNavigator>(FindObjectsSortMode.None).Length;
            if (aliveEnemies < maxAliveEnemies)
            {
                Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position + Vector3.up;
                Instantiate(enemyPrefab, pos, Quaternion.identity);

                ScoreBoardManager scoreBoard = FindAnyObjectByType<ScoreBoardManager>();
                if (scoreBoard != null) scoreBoard.RefreshProgress();
            }
        }
    }
}
