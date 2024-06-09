using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public GameObject[] playerControls; // Boru altýndaki buton gruplarý
    public Button addButton; // ADD PLAYER butonu
    public Button removeButton; // REMOVE PLAYER butonu

    private int currentPlayerIndex = 0;

    void Start()
    {
        // Baþlangýçta sadece ilk oyuncunun butonlarýný aktif yap
        for (int i = 0; i < playerControls.Length; i++)
        {
            playerControls[i].SetActive(i == 0);
        }

        // ADD PLAYER butonuna týklama olayýný baðla
        addButton.onClick.AddListener(AddPlayer);
        // REMOVE PLAYER butonuna týklama olayýný baðla
        removeButton.onClick.AddListener(RemovePlayer);
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