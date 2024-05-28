using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] MinigameManager minigameManager;
    [SerializeField] PlayerManager playerManager;
    [SerializeField] SahneManager sahneManager;
    private void Start()
    {
        instance = this;

        minigameManager.Init();
        playerManager.Init();
        sahneManager.Init();
    }


}
