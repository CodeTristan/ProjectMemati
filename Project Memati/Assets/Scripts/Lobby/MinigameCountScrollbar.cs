using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinigameCountScrollbar : MonoBehaviour
{
    public Scrollbar scrollbar; // Scrollbar referans�
    public TextMeshProUGUI minigameCountText; // TextMeshPro referans�
    public TextMeshProUGUI minigameScreenText; // Minigame Screen'ine ka� tane minigame'nin se�ildi�ini g�nderiyor. Count game objesi bunun i�in

    private bool isDragging = false;
    

    void Start()
    {
        // Scrollbar de�eri de�i�ti�inde UpdateMinigameCount fonksiyonunu �a��r
        scrollbar.onValueChanged.AddListener(delegate { UpdateMinigameCount(); });
        
        // Default de�erini ayarla
        scrollbar.value = 0; // Bu de�eri ba�lang�� olarak ayarlayabilirsin
        UpdateMinigameCount();
    }

    void UpdateMinigameCount()
    {
        // Scrollbar value 0 ile 1 aras�nda de�i�ir, bu de�eri 4-6-8'e d�n��t�r
        if (!isDragging)
        {
            float value = scrollbar.value;

            int minigameCount;
            if (value <= 0.33f)
            {
                minigameCount = 4;
                scrollbar.value = 0;
            }
            else if (value <= 0.66f)
            {
                minigameCount = 6;
                scrollbar.value = 0.5f;
            }
            else
            {
                minigameCount = 8;
                scrollbar.value = 1f;
            }

            minigameCountText.text = minigameCount.ToString();
            minigameScreenText.text = "Minigame Say�s�\n" + minigameCountText.text;

        }
    }

    public void OnPointerDown()
    {
        isDragging = true;
    }

    public void OnPointerUp()
    {
        isDragging = false;
        UpdateMinigameCount();
    }
}
