using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControl;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;


    private MainMenuActions mainMenuActions;
    [SerializeField] private PlayerControl playerPrefab;
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private Vector3 spawnPos;
    private int keyboardPlayerCount = 0;

    [SerializeField] private List<ControlDevice> controlDevices;

    public void Init()
    {
        instance = this;
        controlDevices = new List<ControlDevice>();
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
        //mainMenuActions.CreatePlayer.CreatePlayerGamepad.performed += CreatePlayerGamepad;
    }

    //MainMenu kontrollerini devredýþý býrakýr. Input system.
    public void DisableMainMenuActions()
    {
        mainMenuActions.CreatePlayer.CreatePlayerKeyboard.performed -= CreatePlayerKeyboard;
        //mainMenuActions.CreatePlayer.CreatePlayerGamepad.performed -= CreatePlayerGamepad;
        mainMenuActions.Disable();
        mainMenuActions.CreatePlayer.Disable();
    }

    //Klavye için oyuncu yaratýr. Maksimum 2 tane oyuncu olabilir klavyeden.
    public void CreatePlayerKeyboard(InputAction.CallbackContext context)
    {
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
        
        PlayerControl player = Instantiate(playerPrefab,spawnPos,Quaternion.identity);
    }

    //Gamepad için oyuncu yaratýr.
    public void CreatePlayerGamepad()
    {
        spawnPos += new Vector3(3, 0, 0);
        controlDevices.Add(ControlDevice.Gamepad);
    }
}
