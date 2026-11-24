using UnityEngine;

public class MagicStone : MonoBehaviour
{
    [SerializeField] private int value = 1;
    [SerializeField] private float rotateSpeed = 90f;
    [SerializeField] private GameObject portalToDestroy;
    [SerializeField] private GameObject openVFX;

    void Update()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreBoardManager scoreBoard = FindAnyObjectByType<ScoreBoardManager>();
            if (scoreBoard != null)
            {
                scoreBoard.IncreaseScore(value);
            }

            if (portalToDestroy != null)
            {
                Destroy(portalToDestroy);
                if (openVFX != null)
                {
                    Instantiate(openVFX, portalToDestroy.transform.position, Quaternion.identity);
                }
            }
            

            Destroy(gameObject);
        }
    }
}