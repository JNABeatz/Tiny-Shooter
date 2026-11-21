using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private Slider bar;
    [SerializeField] private float maxValue = 3f;

    void Awake()
    {
        bar = GetComponent<Slider>();
    }

    public void SetMaxValue(float max)
    {
        maxValue = max;
        if (bar != null) bar.value = 1f;
    }

    public void UpdateBar(float points)
    {
        if (bar != null && maxValue > 0f)
        {
            bar.value = points / maxValue;
        }
    }
}
