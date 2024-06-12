using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterScoreSpawner : MonoBehaviour
{
    public static CharacterScoreSpawner instance;
    [SerializeField] private TextMeshProUGUI text;

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
            currentCharacters[i].GetComponent<PlayerControl>().enabled = false;
            currentCharacters[i].transform.Rotate(0, 180, 0);
            
            // Skorlar� ayarla
            scoreTexts[i].text = "Score: " + players[i].score;
        }

        Player winner = players[0];
        if(MinigameManager.instance.selectedMinigames.Count == 0)
        {
            int winnerIndex = 0;
            //winner� bul
            for (int i = 0; i < players.Count; i++)
            {
                if(winner.score < players[i].score)
                {
                    int index = i;
                    winner = players[index];
                    winnerIndex = i;
                }
            }

            text.gameObject.SetActive(true);
            text.text = "Player " + (winnerIndex + 1) + " Wins";
            Camera.main.transform.position = new Vector3(winner.playerControl.transform.position.x,
                                                            Camera.main.transform.position.y,
                                                            winner.playerControl.transform.position.z - 3f);
        }
    }
}
