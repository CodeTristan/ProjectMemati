using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    public GameObject[] sections;
    public int zValue = 70;

    public bool creatingSection = false;
    public int sectionNumber;

    public int count = 0;
    public float StartForwardSpeed;  //baþlangýçtaki ileriye doðru olan hýz

    [Header("Player")]
    [SerializeField] private Transform[] playerSpawnPoints;
    [SerializeField] private PlayerControl PlayerControlPrefab;

    public List<GameObject> spawnedPlayers;

    private void Start()
    {
        spawnedPlayers = new List<GameObject>();
        SpawnPlayers();
    }

    void Update()
    {
        if (!creatingSection & (count < 10)){ //generates 10 sections in total
            creatingSection = true;
            StartCoroutine(GenerateSection());
        }
        
    }

    void SpawnPlayers()
    {
        List<Player> players = PlayerManager.instance.players;

        for (int i = 0; i < players.Count; i++)
        {
            GameObject playerCharacter = Instantiate(players[i].CharacterPrefab, playerSpawnPoints[i].position, Quaternion.identity);
            //playerCharacter.transform.Rotate(0, 180, 0);
            PlayerControl playerControl = playerCharacter.GetComponent<PlayerControl>();
            PlayerMove playerMove = playerCharacter.AddComponent<PlayerMove>();

            //Burada tüm playercontrol özelliklerini prefabdakilerden aktarýyoruz
            playerMove.speed = PlayerControlPrefab.speed;
            playerMove.jumpPower = PlayerControlPrefab.jumpPower;
            playerMove.forwardSpeed = StartForwardSpeed;
            playerMove.Init(players[i].ControlDevice, players[i].device);

            playerControl.enabled = false;
            spawnedPlayers.Add(playerCharacter);
        }
    }

    IEnumerator GenerateSection(){
        sectionNumber = Random.Range(0, 3);
        Instantiate(sections[sectionNumber], new Vector3(0 ,0 , zValue), Quaternion.identity);
        zValue += 70;
        yield return new WaitForSeconds(2);
        count++;
        creatingSection = false;
        
    }
}
