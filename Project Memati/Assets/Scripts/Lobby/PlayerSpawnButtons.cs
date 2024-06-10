using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawnButtons : MonoBehaviour
{
    public GameObject[] playerControls; // Boru altýndaki buton gruplarý


    private int currentPlayerIndex = 0;

    void Start()
    {
        // Baþlangýçta sadece ilk oyuncunun butonlarýný aktif yap
        /*for (int i = 0; i < playerControls.Length; i++)
        {
            playerControls[i].SetActive(i == 0);
        }*/



    }

    void AddPlayer()
    {
        // Sonraki oyuncunun butonlarýný aktif yap
        if (currentPlayerIndex < playerControls.Length - 1)
        {
            currentPlayerIndex++;
            playerControls[currentPlayerIndex].SetActive(true);
        }
    }

    void RemovePlayer()
    {
        // Mevcut oyuncunun butonlarýný gizle
        if (currentPlayerIndex > 0)
        {
            playerControls[currentPlayerIndex].SetActive(false);
            currentPlayerIndex--;
        }
    }
}