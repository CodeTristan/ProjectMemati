using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMinigame : MinigameBase
{

    [SerializeField] private PlayerControl PlayerControlPrefab;
    private List<BombaciControl> spawnedPlayers = new List<BombaciControl>();
    public Transform[] playerSpawnPoints;


    public override void Init()
    {
        
    }

    private void Start()
    {
        SpawnPlayers();
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
}
