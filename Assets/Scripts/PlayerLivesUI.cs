using UnityEngine;
using UnityEngine.UI;

public class PlayerLivesUI : MonoBehaviour
{
    [SerializeField] private Image[] livesBar;

    public void UpdateLivesBar(int lives)
    {
        for (int i = 0; i < livesBar.Length; i++)
        {
            if (livesBar[i] != null)
            {
                livesBar[i].gameObject.SetActive(i < lives);
            }
        }
    }
}