using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] private bool isFinalLevel = false;
    [SerializeField] private GameObject doorModel;
    [SerializeField] private bool destroyOnOpen = false;
    [SerializeField] private GameObject openVFX; // Opcional: arrastra FireExplosionVFX

    private bool isOpen = false;

    public void OpenDoor()
    {
        if (isOpen) return;
        isOpen = true;

        if (doorModel == null) return;

        if (openVFX != null)
        {
            Instantiate(openVFX, doorModel.transform.position, Quaternion.identity);
        }

        if (destroyOnOpen)
        {
            doorModel.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isOpen || !other.CompareTag("Player")) return;

        if (isFinalLevel)
        {
            VictoryUI victoryUI = FindAnyObjectByType<VictoryUI>();
            if (victoryUI != null) victoryUI.ShowVictory();
        }
        else
        {
            LevelController levelController = FindAnyObjectByType<LevelController>();
            if (levelController != null) levelController.LoadNextLevel();
        }
    }
}