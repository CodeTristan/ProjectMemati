using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawnButtons : MonoBehaviour
{
    public GameObject[] playerControls; // Boru altındaki buton grupları


    private int currentPlayerIndex = 0;

    void Start()
    {


    }

    void AddPlayer()
    {
        // Sonraki oyuncunun butonlarını aktif yap
        if (currentPlayerIndex < playerControls.Length - 1)
        {
            currentPlayerIndex++;
            playerControls[currentPlayerIndex].SetActive(true);
        }
    }

    void RemovePlayer()
    {
        // Mevcut oyuncunun butonlarını gizle
        if (currentPlayerIndex > 0)
        {
            playerControls[currentPlayerIndex].SetActive(false);
            currentPlayerIndex--;
        }
    }
}