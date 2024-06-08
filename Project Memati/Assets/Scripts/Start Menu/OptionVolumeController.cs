using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class VolumeController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumeSlider;
    public TextMeshProUGUI percentageText; // Text yerine TextMeshProUGUI kullanýyorsanýz

    void Start()
    {
        // Slider'ýn baþlangýç deðerini 100 olarak ayarlayýn
        int volume = PlayerPrefs.GetInt("Volume", 100);
        volumeSlider.value = volume;
        SetVolume(volume);
        UpdatePercentageText(volume);
    }

    public void SetVolume(float sliderValue)
    {
        // Slider deðerini 0 ile 1 arasýna dönüþtürün
        float volume = sliderValue / 100f;
        audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetInt("Volume", (int)sliderValue);
        UpdatePercentageText((int)sliderValue);
    }

    private void UpdatePercentageText(int sliderValue)
    {
        percentageText.text = sliderValue.ToString() + "%";
    }
}