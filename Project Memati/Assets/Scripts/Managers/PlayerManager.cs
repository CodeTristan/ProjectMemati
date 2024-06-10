using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControl;

[System.Serializable]
public class Player
{
    public PlayerControl playerControl;
    public GameObject CharacterPrefab;
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
    [SerializeField] private List<PlayerControl> players;

    public void Init()
    {
        instance = this;
        controlDevices = new List<ControlDevice>();
        players = new List<PlayerControl>();
        mainMenuActions = new MainMenuActions();
        EnableMainMenuActions();
    }

    //Change it to another manager later on
    public void _StartGame()
    {
        PlayerControl[] players = FindObjectsOfType<PlayerControl>();
        for (int i = 0; i < players.Length; i++)
        {
            players[i].Init(controlDevices[i]);
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
            return;

        controlDevices.Add(controlDevice);
        PlayerControl player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        player.device = context.control.device;
        players.Add(player);
        player.Init(controlDevice);
        spawnPos += new Vector3(3, 0, 0);
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
        PlayerControl player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        player.device = context.control.device;
        players.Add(player);
        player.Init(ControlDevice.Gamepad);
        controlDevices.Add(ControlDevice.Gamepad);
    }
}
