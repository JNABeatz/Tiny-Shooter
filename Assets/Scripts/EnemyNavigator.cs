using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyNavigator : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform fireStartPos;
    [SerializeField] private float fireRate = 2f;
    private NavMeshAgent navMeshAgent;
    private MoverJumper player;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<MoverJumper>();
        if (player != null)
        {
            navMeshAgent.SetDestination(player.transform.position);
            StartCoroutine(FireRoutine());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        navMeshAgent.SetDestination(player.transform.position);
        Vector3 lookTarget = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(lookTarget);
    }

    private IEnumerator FireRoutine()
    {
        while (player != null)
        {
            yield return new WaitForSeconds(fireRate);
            if (player == null) yield break;
            GameObject newProjectile = Instantiate(projectilePrefab, fireStartPos.position, transform.rotation);
            newProjectile.transform.LookAt(new Vector3(player.transform.position.x, fireStartPos.position.y, player.transform.position.z));
        }
    }
}
