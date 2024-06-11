using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnButtons : MonoBehaviour
{
    public static PlayerSpawnButtons instance;
    public GameObject[] playerControls; // Boru alt�ndaki buton gruplar�
    private int currentActiveIndex = 0; // �u anda aktif olan butonun indeksi

    void Start()
    {
        instance = this;
        // Ba�lang��ta t�m butonlar� devre d��� b�rak�yoruz
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
