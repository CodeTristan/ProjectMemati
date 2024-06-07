using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //GameManager tüm managerlarý sýrayla çaðýran en üstteki yöneticidir. Ýçerisinde tüm managerlar bulunur.
    public static GameManager instance;

    [SerializeField] MinigameManager minigameManager;
    [SerializeField] PlayerManager playerManager;
    [SerializeField] SahneManager sahneManager;

    //Oyundaki tek start fonksiyonu buradadýr. Diðer managerlarda Init fonksiyonu bulunur ve burada istenilen sýraya göre çaðýrýlýr.
    private void Start()
    {
        instance = this;

        minigameManager.Init();
        playerManager.Init();
        sahneManager.Init();
    }


}
