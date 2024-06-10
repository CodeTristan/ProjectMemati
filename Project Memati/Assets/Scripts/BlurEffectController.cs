using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class BlurEffectController : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;
    public Button startButton;
    public Button backButton;

    private DepthOfField depthOfField;

    void Start()
    {
        // Post Process Volume'dan Depth of Field efektini al
        postProcessVolume.profile.TryGetSettings(out depthOfField);

        // Ba�lang��ta Depth of Field efekti kapal�
        depthOfField.active = false;

        // Depth of Field ayarlar�
        depthOfField.focusDistance.value = 2f;
        depthOfField.aperture.value = 5.6f;
        depthOfField.focalLength.value = 50f;

        // Butonlara listener ekle
        startButton.onClick.AddListener(ApplyBlur);
        backButton.onClick.AddListener(ClearBlur);
    }

    void ApplyBlur()
    {
        // Bulan�kl�k efekti uygula
        depthOfField.active = true;
    }

    void ClearBlur()
    {
        // Bulan�kl�k efektini temizle
        depthOfField.active = false;
    }
}
