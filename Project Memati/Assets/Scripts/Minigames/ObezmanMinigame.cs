using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObezmanMinigame : MinigameBase
{
    public GameObject burger;

    public Vector3 firstPosition;

    public int score = 0;


    
    public int iks = 5;
    public int ye = 5;
    public float bosluk = 2.0f;
    public override void Init()
    {
    }

    private void Start()
    {
        SpawnBurgers();

    }

    void SpawnBurgers()
    {
        Vector3 startPosition = firstPosition;

        for (int i = 0; i < iks; i++)
        {
            for (int j = 0; j < ye; j++)
            {
                Vector3 spawnPosition = startPosition + new Vector3(i*bosluk, 0, j*bosluk);

                Instantiate(burger, spawnPosition, Quaternion.identity);
            }
        }
    }
}
