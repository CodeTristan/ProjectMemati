using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnButtons : MonoBehaviour
{
    public static PlayerSpawnButtons instance;
    public GameObject[] playerControls; // Boru altýndaki buton gruplarý
    private int currentActiveIndex = 0; // Þu anda aktif olan butonun indeksi

    void Start()
    {
        instance = this;
        // Baþlangýçta tüm butonlarý devre dýþý býrakýyoruz
        foreach (GameObject control in playerControls)
        {
            control.SetActive(false);
        }
    }

    public void ActivateNextButtonGroup()
    {
        // Sonraki buton grubunu aktif hale getir
        if (currentActiveIndex < playerControls.Length)
        {
            playerControls[currentActiveIndex].SetActive(true);
            currentActiveIndex++;
        }
    }
}
