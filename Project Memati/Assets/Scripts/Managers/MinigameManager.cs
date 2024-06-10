using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager instance;

    public int MaxMinigameCount;
    public string[] minigames;

    public List<string> selectedMinigames;
    
    public void Init()
    {
        instance = this;
        selectedMinigames = new List<string>();
       
    }


    //Rastgele �ekilde MaxCount kadar minigameleri se�er ve selectedMinigames listesine ekler. T�m minigamelerden 1 tane olunca tekrar listeyi doldurur.
    public void SelectMinigames()
    {
        selectedMinigames.Clear();
        //Ui ile implemente edilecek.
        List<string> minigameBases = minigames.ToList();
        for (int i = 0; i < MaxMinigameCount; i++)
        {
            int random = Random.Range(0, minigameBases.Count);
            selectedMinigames.Add(minigameBases[random]);
            minigameBases.RemoveAt(random);
            if(minigameBases.Count == 0)
            {
                minigameBases = minigames.ToList(); //Minigameleri tekrar listeye ekler ve Full liste i�erisinden se�ilmeye devam eder.
            }
        }
    }
}
