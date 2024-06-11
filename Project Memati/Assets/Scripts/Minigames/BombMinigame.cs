using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BombMinigame : MinigameBase
{

    [SerializeField] private PlayerControl PlayerControlPrefab;
    [SerializeField] private GameObject bombPrefab;
    private List<BombaciControl> spawnedPlayers = new List<BombaciControl>();
    public Transform[] playerSpawnPoints;
    public Transform[] bombSpawnPoints;
    private Transform bombSpawnPoint;

    public float timeRemaining = 10;
    public TextMeshProUGUI timeText;
    public bool timerIsRunning = false;

    public BombaciControl winner;

    public override void Init()
    {
        
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                EndGame();
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    void EndGame()
    {

        List<Player> players = PlayerManager.instance.players;
        foreach (BombaciControl player in spawnedPlayers)
        {
            if (player.hasbomb)
            {
                winner = player;
            }
        }

        int anan = spawnedPlayers.IndexOf(winner);

        players[anan].score += 20;
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = "Time Left\n" + string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void Start()
    {
        SpawnBomb();
        SpawnPlayers();
        timerIsRunning = true;
        winner = spawnedPlayers[0];
    }

    void SpawnPlayers()
    {
        List<Player> players = PlayerManager.instance.players;

        for (int i = 0; i < players.Count; i++)
        {
            GameObject playerCharacter = Instantiate(players[i].CharacterPrefab, playerSpawnPoints[i].position, Quaternion.identity);
            BombaciControl bombaciControl = playerCharacter.AddComponent<BombaciControl>();
            playerCharacter.name = "player" + string.Format("{0}", i);
            playerCharacter.transform.Rotate(0, 180, 0);
            PlayerControl playerControl = playerCharacter.GetComponent<PlayerControl>();

            //Burada tüm playercontrol özelliklerini prefabdakilerden aktarıyoruz
            bombaciControl.speed = PlayerControlPrefab.speed;
            bombaciControl.jumpPower = PlayerControlPrefab.jumpPower;
            bombaciControl.Init(players[i].ControlDevice, players[i].device);


            playerControl.enabled = false;
            spawnedPlayers.Add(bombaciControl);
            
        }
    }

    void SpawnBomb()
    {
        int randomIndex = Random.Range(0, playerSpawnPoints.Length);
        bombSpawnPoint = playerSpawnPoints[randomIndex];
        Instantiate(bombPrefab, bombSpawnPoint.position, Quaternion.identity);
    }
}
