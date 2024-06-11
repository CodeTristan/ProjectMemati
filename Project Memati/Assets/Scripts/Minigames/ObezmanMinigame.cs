using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObezmanMinigame : MinigameBase
{
    public GameObject burger;

    public Vector3 firstPosition;

    public int score = 0;

    public GameObject[] mazePrefabs;
    public Transform[] spawnPoints;

    public Transform[] playerSpawnPoints;
    [SerializeField] private PlayerControl PlayerControlPrefab;
    private List<GameObject> spawnedPlayers = new List<GameObject>();

    public int iks = 5;
    public int ye = 5;
    public float bosluk = 2.0f;

    private List<GameObject> spawnedMazes = new List<GameObject>();
    private float switchTime = 5.0f;
    private float moveDuration = 1.0f;
    public override void Init()
    {

    }

    private void Start()
    {
        SpawnBurgers();
        SpawnInitialMaze();
        StartCoroutine(SwitchMazes());
        SpawnPlayers();
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
            playerCharacter.transform.Rotate(0, 180, 0);
            PlayerControl playerControl = playerCharacter.GetComponent<PlayerControl>();

            //Burada tüm playercontrol özelliklerini prefabdakilerden aktarıyoruz
            playerControl.speed = PlayerControlPrefab.speed;
            playerControl.jumpPower = PlayerControlPrefab.jumpPower;
            playerControl.Init(players[i].ControlDevice, players[i].device);


            spawnedPlayers.Add(playerCharacter);
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
