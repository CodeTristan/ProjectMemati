using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    [SerializeField] private CameraMove CameraMove;
    public GameObject[] sections;
    public int zValue = 70;

    public bool creatingSection = false;
    public int sectionNumber;

    public int count = 0;


    public float StartForwardSpeed;  //baþlangýçtaki ileriye doðru olan hýz
    public float StartTimer;

    private float currentTimer;
    [SerializeField] private TextMeshProUGUI StartTimerText;

    [Header("Player")]
    [SerializeField] private Transform[] playerSpawnPoints;
    [SerializeField] private PlayerControl PlayerControlPrefab;

    public List<PlayerMove> spawnedPlayers;

    private void Start()
    {
        spawnedPlayers = new List<PlayerMove>();
        currentTimer = StartTimer;
        SpawnPlayers();

    }

    void Update()
    {
        if (!creatingSection & (count < 10)){ //generates 10 sections in total
            creatingSection = true;
            StartCoroutine(GenerateSection());
        }
        currentTimer -= Time.deltaTime;
        if(currentTimer <= 0)
        {
            StartTimerText.gameObject.SetActive(false);
            StartTheGame();
            currentTimer = 99999;
        }
        else
        {
            StartTimerText.text = currentTimer.ToString();
        }
    }

    void StartTheGame()
    {
        List<Player> players = PlayerManager.instance.players;

        for (int i = 0; i < players.Count; i++)
        {
            spawnedPlayers[i].Init(players[i].ControlDevice, players[i].device);
            spawnedPlayers[i].startMove = true;
        }
        CameraMove.Init();

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
            playerMove.device = players[i].device;
            playerMove.speed = PlayerControlPrefab.speed;
            playerMove.jumpPower = PlayerControlPrefab.jumpPower;
            playerMove.forwardSpeed = StartForwardSpeed;

            playerControl.enabled = false;
            spawnedPlayers.Add(playerMove);
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
