using UnityEngine;
using UnityEngine.UI;

public class SliderGradient : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Gradient gradient;

    private void Awake()
    {
        slider.onValueChanged.AddListener(UpdateColor);
        UpdateColor(slider.value);
    }

    private void UpdateColor(float value)
    {
        float t = slider.normalizedValue; // 0–1
        fillImage.color = gradient.Evaluate(t);
    }
}
