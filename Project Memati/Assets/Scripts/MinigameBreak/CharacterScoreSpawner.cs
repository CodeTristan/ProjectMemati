using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterScoreSpawner : MonoBehaviour
{
    public static CharacterScoreSpawner instance;

    public Transform[] spawnPoints; // Karakterlerin spawn olaca�� noktalar (borular�n �st�)
    public TextMeshProUGUI[] scoreTexts; // Skorlar� g�sterecek TextMeshProUGUI bile�enleri

    private GameObject[] currentCharacters; // Mevcut karakterler

    void Start()
    {
        instance = this;
        List<Player> players = PlayerManager.instance.players;

        currentCharacters = new GameObject[spawnPoints.Length];

        // Oyuncular� ve skorlar� ba�lat
        for (int i = 0; i < players.Count; i++)
        {
            // Karakteri spawnla
            currentCharacters[i] = Instantiate(players[i].CharacterPrefab, spawnPoints[i].position, Quaternion.identity);
            currentCharacters[i].transform.Rotate(0, 180, 0);
            
            // Skorlar� ayarla
            scoreTexts[i].text = "Score: " + players[i].score;
        }
    }
}
