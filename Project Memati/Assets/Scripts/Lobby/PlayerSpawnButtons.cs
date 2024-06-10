using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawnButtons : MonoBehaviour
{
    public GameObject[] playerControls; // Boru alt�ndaki buton gruplar�


    private int currentPlayerIndex = 0;

    void Start()
    {
        // Ba�lang��ta sadece ilk oyuncunun butonlar�n� aktif yap
        /*for (int i = 0; i < playerControls.Length; i++)
        {
            playerControls[i].SetActive(i == 0);
        }*/



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