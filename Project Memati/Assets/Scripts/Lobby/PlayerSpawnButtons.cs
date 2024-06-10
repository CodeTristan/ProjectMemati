using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnButtons : MonoBehaviour
{
    public GameObject[] playerControls; // Boru altýndaki buton gruplarý
    private int currentActiveIndex = 0; // Þu anda aktif olan butonun indeksi

    void Start()
    {
        // Baþlangýçta tüm butonlarý devre dýþý býrakýyoruz
        foreach (GameObject control in playerControls)
        {
            control.SetActive(false);
        }
    }

    void Update()
    {
        // Enter tuþuna basýldýðýnda sýradaki butonu aktif hale getir
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            ActivateNextButtonGroup();
        }
    }

    void ActivateNextButtonGroup()
    {
        // Sonraki buton grubunu aktif hale getir
        if (currentActiveIndex < playerControls.Length)
        {
            playerControls[currentActiveIndex].SetActive(true);
            currentActiveIndex++;
        }
    }
}
