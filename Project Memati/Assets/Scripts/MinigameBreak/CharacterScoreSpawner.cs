using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterScoreSpawner : MonoBehaviour
{
    public static CharacterScoreSpawner instance;

    public Transform[] spawnPoints; // Karakterlerin spawn olacaðý noktalar (borularýn üstü)
    public TextMeshProUGUI[] scoreTexts; // Skorlarý gösterecek TextMeshProUGUI bileþenleri

    private GameObject[] currentCharacters; // Mevcut karakterler

    void Start()
    {
        instance = this;
        List<Player> players = PlayerManager.instance.players;

        currentCharacters = new GameObject[spawnPoints.Length];

        // Oyuncularý ve skorlarý baþlat
        for (int i = 0; i < players.Count; i++)
        {
            // Karakteri spawnla
            currentCharacters[i] = Instantiate(players[i].CharacterPrefab, spawnPoints[i].position, Quaternion.identity);
            currentCharacters[i].transform.Rotate(0, 180, 0);
            
            // Skorlarý ayarla
            scoreTexts[i].text = "Score: " + players[i].score;
        }
    }
}
