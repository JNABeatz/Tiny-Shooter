using UnityEngine;

public class Switch : MonoBehaviour
{

    private bool isUnlocked = false;
    private bool isActivated = false;

    void Awake()
    {
        if (!isUnlocked) gameObject.SetActive(false);
    }

    public void UnlockSwitch()
    {
        if (isUnlocked) return;
        isUnlocked = true;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isUnlocked || isActivated || !other.CompareTag("Player")) return;

        isActivated = true;

        ExitDoor exitDoor = FindAnyObjectByType<ExitDoor>();
        if (exitDoor != null)
        {
            exitDoor.OpenDoor();
        }

        ScoreBoardManager scoreBoard = FindAnyObjectByType<ScoreBoardManager>();
        if (scoreBoard != null)
        {
            scoreBoard.SetDoorOpened();
        }

        Destroy(gameObject);
    }
}