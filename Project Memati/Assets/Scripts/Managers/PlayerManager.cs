using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControl;

[System.Serializable]
public class Player
{
    public int playerID;
    public PlayerControl playerControl;
    public ControlDevice ControlDevice;
    public InputDevice device;
    public GameObject CharacterPrefab;
    public int score;
}

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    private MainMenuActions mainMenuActions;
    [SerializeField] private PlayerControl playerPrefab;
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private Vector3 spawnPos;
    private int MaxPlayerCount = 4;
    private int keyboardPlayerCount = 0;

    [SerializeField] private List<ControlDevice> controlDevices;
    public List<Player> players;

    public void Init()
    {
        instance = this;
        controlDevices = new List<ControlDevice>();
        players = new List<Player>();
        mainMenuActions = new MainMenuActions();
    }

    public void SpawnPlayers(Vector3[] spawnPoints)
    {
        for (int i = 0; i < players.Count; i++)
        {
            Instantiate(players[i].CharacterPrefab, spawnPoints[i], Quaternion.identity);
            players[i].playerControl.Init(controlDevices[i], players[i].device);
        }
    }

    //MainMenu kontrollerini etkinleþtirir. Input system.
    public void EnableMainMenuActions()
    {
        mainMenuActions.Enable();
        mainMenuActions.CreatePlayer.Enable();
        mainMenuActions.CreatePlayer.CreatePlayerKeyboard.performed += CreatePlayerKeyboard;
        mainMenuActions.CreatePlayer.CreatePlayerGamepad.performed += CreatePlayerGamepad;
    }

    //MainMenu kontrollerini devredýþý býrakýr. Input system.
    public void DisableMainMenuActions()
    {
        mainMenuActions.CreatePlayer.CreatePlayerKeyboard.performed -= CreatePlayerKeyboard;
        mainMenuActions.CreatePlayer.CreatePlayerGamepad.performed -= CreatePlayerGamepad;
        mainMenuActions.Disable();
        mainMenuActions.CreatePlayer.Disable();
    }

    //Klavye için oyuncu yaratýr. Maksimum 2 tane oyuncu olabilir klavyeden.
    public void CreatePlayerKeyboard(InputAction.CallbackContext context)
    {
        if (players.Count >= 4)
            return;
        keyboardPlayerCount++;
        ControlDevice controlDevice = new ControlDevice();

        //setting enum according to player count with keyboard
        if (keyboardPlayerCount == 1)
            controlDevice = ControlDevice.KeyboardLeft;
        else if (keyboardPlayerCount == 2)
            controlDevice = ControlDevice.KeyboardRight;
        else
            controlDevice = ControlDevice.KeyboardLeft;

        controlDevices.Add(controlDevice);
        Player player = new Player();
        player.device = context.control.device;
        players.Add(player);
        player.ControlDevice = controlDevice;
        //player.playerControl.Init(controlDevice);
        spawnPos += new Vector3(3, 0, 0);

        CharacterSpawner.instance.SpawnCharacterAt(players.Count - 1, 0);
        player.playerControl = player.CharacterPrefab.GetComponent<PlayerControl>();
        player.playerControl.player = player;
        PlayerSpawnButtons.instance.ActivateNextButtonGroup();
    }

    //Gamepad için oyuncu yaratýr.
    public void CreatePlayerGamepad(InputAction.CallbackContext context)
    {
        foreach (var item in players)
        {
            if (item.device.deviceId == context.control.device.deviceId)
            {
                Debug.Log("The device is already in the game : " + item.device.name);
                return;
            }
        }
        if (players.Count >= 4)
            return;
        spawnPos += new Vector3(3, 0, 0);
        Player player = new Player();
        player.device = context.control.device;
        player.ControlDevice = ControlDevice.Gamepad;
        players.Add(player);
        //player.playerControl.Init(ControlDevice.Gamepad);
        controlDevices.Add(ControlDevice.Gamepad);

        CharacterSpawner.instance.SpawnCharacterAt(players.Count - 1, 0);
        player.playerControl = player.CharacterPrefab.GetComponent<PlayerControl>();
        player.playerControl.player = player;
        PlayerSpawnButtons.instance.ActivateNextButtonGroup();
    }
}
