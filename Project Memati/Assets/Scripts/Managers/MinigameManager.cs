using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager instance;

    public int MaxMinigameCount;
    public MinigameBase[] minigames;

    private List<MinigameBase> selectedMinigames;
    
    public void Init()
    {
        instance = this;
        selectedMinigames = new List<MinigameBase>();
       
    }

    public void SelectMinigames()
    {
        //Ui ile implemente edilecek.
        List<MinigameBase> minigameBases = minigames.ToList();
        for (int i = 0; i < MaxMinigameCount; i++)
        {
            int random = Random.Range(0, minigameBases.Count);
            selectedMinigames.Add(minigameBases[random]);
            minigameBases.RemoveAt(random);
            if(minigameBases.Count == 0)
            {
                minigameBases = minigames.ToList();
            }
        }
    }
}
