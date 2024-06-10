using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinigameCountScrollbar : MonoBehaviour
{
    public Scrollbar scrollbar; // Scrollbar referansý
    public TextMeshProUGUI minigameCountText; // TextMeshPro referansý
    public TextMeshProUGUI minigameScreenText; // Minigame Screen'ine kaç tane minigame'nin seçildiðini gönderiyor. Count game objesi bunun için

    private bool isDragging = false;
    

    void Start()
    {
        // Scrollbar deðeri deðiþtiðinde UpdateMinigameCount fonksiyonunu çaðýr
        scrollbar.onValueChanged.AddListener(delegate { UpdateMinigameCount(); });
        
        // Default deðerini ayarla
        scrollbar.value = 0; // Bu deðeri baþlangýç olarak ayarlayabilirsin
        UpdateMinigameCount();
    }

    void UpdateMinigameCount()
    {
        // Scrollbar value 0 ile 1 arasýnda deðiþir, bu deðeri 4-6-8'e dönüþtür
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
            minigameScreenText.text = "Minigame Sayýsý\n" + minigameCountText.text;

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
