using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    public static GenerateLevel instance;
    [SerializeField] private CameraMove CameraMove;
    public GameObject[] sections;

    public int zValue = 70;

    public bool creatingSection = false;
    public int sectionNumber;

    public int count = 0;

    public int playerCount;
    public float StartForwardSpeed;  //ba�lang��taki ileriye do�ru olan h�z
    public float StartTimer;

    private float currentTimer;
    [SerializeField] private TextMeshProUGUI StartTimerText;

    [Header("Player")]
    [SerializeField] private Transform[] playerSpawnPoints;
    [SerializeField] private PlayerControl PlayerControlPrefab;

    public List<PlayerMove> spawnedPlayers;

    public List<Player> winners; //oyuncuların oyunu kazanma sirasi tutulur

    private void Start()
    {
        instance = this;
        spawnedPlayers = new List<PlayerMove>();
        currentTimer = StartTimer;
        SpawnPlayers();
        winners = new List<Player>();

    }

    void Update()
    {
        if (!creatingSection & (count < 4)){ //generates 10 sections in total
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

        if(playerCount <= 0){
            playerCount = 100;
            foreach(var w in winners){
                w.score+= 20;
            }
                SahneManager.instance.LoadScene("MinigameBreak");
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

            Animator playerAnimator = playerCharacter.GetComponent<Animator>();
            playerAnimator.Play("Fast Run");


            //Burada t�m playercontrol �zelliklerini prefabdakilerden aktar�yoruz
            playerMove.device = players[i].device;
            playerMove.speed = PlayerControlPrefab.speed;
            playerMove.jumpPower = PlayerControlPrefab.jumpPower;
            playerMove.forwardSpeed = StartForwardSpeed;


            playerControl.enabled = false;
            spawnedPlayers.Add(playerMove);

            playerCount++;
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
