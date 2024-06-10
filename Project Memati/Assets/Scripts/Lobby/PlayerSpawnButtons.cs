using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnButtons : MonoBehaviour
{
    public GameObject[] playerControls; // Boru alt�ndaki buton gruplar�
    private int currentActiveIndex = 0; // �u anda aktif olan butonun indeksi

    void Start()
    {
        // Ba�lang��ta t�m butonlar� devre d��� b�rak�yoruz
        foreach (GameObject control in playerControls)
        {
            control.SetActive(false);
        }
    }

    void Update()
    {
        // Enter tu�una bas�ld���nda s�radaki butonu aktif hale getir
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
