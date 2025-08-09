using UnityEngine;
using UnityEngine.UI;

public class LogicShine : MonoBehaviour
{
    public Slider slider;
    public Image imageShine;
    void Start()
    {
        float shineValue = PlayerPrefs.GetFloat("Shine", 0.5f);
        slider.value = shineValue;
        SetImageAlpha(1 - shineValue);
        slider.onValueChanged.AddListener(ChangeSlider);
    }

    public void ChangeSlider(float value)
    {
        PlayerPrefs.SetFloat("Shine", value);
        SetImageAlpha(1 - value);
    }

    private void SetImageAlpha(float alpha)
    {
        Color color = imageShine.color;
        imageShine.color = new Color(color.r, color.g, color.b, alpha);
    }
}
