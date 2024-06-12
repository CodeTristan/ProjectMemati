using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ObezmanMinigame : MinigameBase
{
    public GameObject burger;

    public float timeRemaining = 10;
    public TextMeshProUGUI timeText;
    public bool timerIsRunning = false;

    public Vector3 firstPosition;

    public int score = 0;

    public GameObject[] mazePrefabs;
    public Transform[] spawnPoints;

    public Transform[] playerSpawnPoints;
    [SerializeField] private PlayerControl PlayerControlPrefab;
    private List<ObezmanControl> spawnedPlayers = new List<ObezmanControl>();

    public int iks = 5;
    public int ye = 5;
    public float bosluk = 2.0f;

    private List<GameObject> spawnedMazes = new List<GameObject>();
    private float switchTime = 5.0f;
    private float moveDuration = 1.0f;

    public List<Camera> playerCameras;

    public TextMeshProUGUI[] scoreTexts;

    public ObezmanControl winner;

    public override void Init()
    {

    }

    private void Start()
    {
        SpawnBurgers();
        SpawnInitialMaze();
        StartCoroutine(SwitchMazes());
        SpawnPlayers();
        AdjustCameraViewports();
        timerIsRunning = true;
        winner = spawnedPlayers[0];
        AdjustScoreTexts();
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
        foreach (ObezmanControl player in spawnedPlayers)
        {
            if (winner.sikor < player.sikor)
            {
                winner = player;
            }
        }

        int anan = spawnedPlayers.IndexOf(winner);

        players[anan].score += 20;
        SahneManager.instance.LoadScene("MinigameBreak");
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = "Time Left\n" + string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void SpawnBurgers()
    {
        Vector3 startPosition = firstPosition;

        for (int i = 0; i < iks; i++)
        {
            for (int j = 0; j < ye; j++)
            {
                Vector3 spawnPosition = startPosition + new Vector3(i * bosluk, 0, j * bosluk);

                Instantiate(burger, spawnPosition, Quaternion.identity);
            }
        }
    }

    void SpawnPlayers()
    {
        List<Player> players = PlayerManager.instance.players;

        for (int i = 0; i < players.Count; i++)
        {
            GameObject playerCharacter = Instantiate(players[i].CharacterPrefab, playerSpawnPoints[i].position, Quaternion.identity);
            ObezmanControl obezmanControl = playerCharacter.AddComponent<ObezmanControl>();
            obezmanControl.scoreText = scoreTexts[i];
            obezmanControl.playername=(i+1).ToString();
            playerCharacter.name = "player" + string.Format("{0}", i);
            playerCharacter.transform.Rotate(0, 180, 0);
            PlayerControl playerControl = playerCharacter.GetComponent<PlayerControl>();

            //animasyonu kosuya gecir
            Animator playerAnimator = playerCharacter.GetComponent<Animator>();
            playerAnimator.Play("Fast Run");

            //Burada tüm playercontrol özelliklerini prefabdakilerden aktarıyoruz
            obezmanControl.speed = PlayerControlPrefab.speed;
            obezmanControl.jumpPower = PlayerControlPrefab.jumpPower;
            obezmanControl.Init(players[i].ControlDevice, players[i].device);

            obezmanControl.playerCamera = playerCameras[i];

            playerControl.enabled = false;
            spawnedPlayers.Add(obezmanControl);

            FollowPlayer followPlayerScript = playerCameras[i].GetComponent<FollowPlayer>();
            if (followPlayerScript != null)
            {
                followPlayerScript.playerTransform = playerCharacter.transform;
                playerCameras[i].transform.position = playerCharacter.transform.position + followPlayerScript.offset;
                playerCameras[i].transform.LookAt(playerCharacter.transform);
            }
        }
    }

    void AdjustScoreTexts()
    {
        int playercount = spawnedPlayers.Count;


        if (playercount == 1)
        {
            // scoreTexts[i].transform.localPosition=
        }
        if (playercount == 2)
        {

            scoreTexts[1].rectTransform.anchoredPosition = new Vector3(-1804, 126, 0);
        }
        if (playercount == 3)
        {
            scoreTexts[1].rectTransform.anchoredPosition = new Vector3(-1804, 126, 0);
            scoreTexts[2].rectTransform.anchoredPosition = new Vector3(-115, 126, 0);
        }
        if (playercount == 4)
        {
        }
    }

    void AdjustCameraViewports()
    {
        int playercount = spawnedPlayers.Count;
        for (int i = 0; i < playercount; i++)
        {
            if (playercount == 1)
            {
                playerCameras[i].rect = new Rect(0, 0, 1, 1);
            }

            else if (playercount == 2)
            {
                playerCameras[i].rect = new Rect(0, 0.5f * i, 1, 0.5f);
            }

            else if (playercount == 3)
            {
                if (i == 0)
                {
                    playerCameras[i].rect = new Rect(0, 0.5f, 1, 0.5f);
                }
                else
                {
                    playerCameras[i].rect = new Rect(0.5f * (i - 1), 0, 0.5f, 0.5f);
                }
            }

            else if (playercount == 4)
            {
                playerCameras[0].rect = new Rect(0, 0.5f, 0.5f, 0.5f); 
                playerCameras[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f); 
                playerCameras[2].rect = new Rect(0, 0, 0.5f, 0.5f); 
                playerCameras[3].rect = new Rect(0.5f, 0, 0.5f, 0.5f); 
            }
        }
    }


    void SpawnInitialMaze()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        int randomIndex = Random.Range(0, mazePrefabs.Length);
        GameObject maze = Instantiate(mazePrefabs[randomIndex], spawnPoint.position, Quaternion.identity);

        spawnedMazes.Add(maze);
    }
    IEnumerator SwitchMazes()
    {
        while (true)
        {
            yield return new WaitForSeconds(switchTime);
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            int randomIndex = Random.Range(0, mazePrefabs.Length);
            GameObject newMaze = Instantiate(mazePrefabs[randomIndex], spawnPoint.position - new Vector3(0, 10, 0), Quaternion.identity);
            yield return StartCoroutine(MoveMazes(newMaze));
        }
    }

    IEnumerator MoveMazes(GameObject newMaze)
    {
        float elapsedTime = 0;
        List<Vector3> startPositions = new List<Vector3>();
        List<Vector3> endPositions = new List<Vector3>();

        foreach (var labirent in spawnedMazes)
        {
            startPositions.Add(labirent.transform.position);
            endPositions.Add(labirent.transform.position - new Vector3(0, 10, 0));
        }

        Vector3 newMazeStartPosition = newMaze.transform.position;
        Vector3 newMazeEndPosition = newMaze.transform.position + new Vector3(0, 10, 0);

        while (elapsedTime < moveDuration)
        {
            for (int i = 0; i < spawnedMazes.Count; i++)
            {
                spawnedMazes[i].transform.position = Vector3.Lerp(startPositions[i], endPositions[i], elapsedTime / moveDuration);
            }

            newMaze.transform.position = Vector3.Lerp(newMazeStartPosition, newMazeEndPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < spawnedMazes.Count; i++)
        {
            spawnedMazes[i].transform.position = endPositions[i];
            Destroy(spawnedMazes[i]);
        }

        newMaze.transform.position = newMazeEndPosition;
        spawnedMazes.Clear();
        spawnedMazes.Add(newMaze);
    }


}
