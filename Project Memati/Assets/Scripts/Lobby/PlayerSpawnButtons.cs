using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawnButtons : MonoBehaviour
{
    public GameObject[] playerControls; // Boru alt�ndaki buton gruplar�


    private int currentPlayerIndex = 0;

    void Start()
    {


    }

    void AddPlayer()
    {
        // Sonraki oyuncunun butonlar�n� aktif yap
        if (currentPlayerIndex < playerControls.Length - 1)
        {
            currentPlayerIndex++;
            playerControls[currentPlayerIndex].SetActive(true);
        }
    }

    void RemovePlayer()
    {
        // Mevcut oyuncunun butonlar�n� gizle
        if (currentPlayerIndex > 0)
        {
            playerControls[currentPlayerIndex].SetActive(false);
            currentPlayerIndex--;
        }
    }
}