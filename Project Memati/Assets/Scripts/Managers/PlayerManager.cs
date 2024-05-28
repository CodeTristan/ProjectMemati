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

    //Enables mainmenu actions input system 
    public void EnableMainMenuActions()
    {
        mainMenuActions.Enable();
        mainMenuActions.CreatePlayer.Enable();
        mainMenuActions.CreatePlayer.CreatePlayerKeyboard.performed += CreatePlayerKeyboard;
        //mainMenuActions.CreatePlayer.CreatePlayerGamepad.performed += CreatePlayerGamepad;
    }

    //Disables mainmenu actions input system 
    public void DisableMainMenuActions()
    {
        mainMenuActions.CreatePlayer.CreatePlayerKeyboard.performed -= CreatePlayerKeyboard;
        //mainMenuActions.CreatePlayer.CreatePlayerGamepad.performed -= CreatePlayerGamepad;
        mainMenuActions.Disable();
        mainMenuActions.CreatePlayer.Disable();
    }

    //Creates player for keyboard. Max 2 Players allowed
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

    //
    public void CreatePlayerGamepad()
    {
        spawnPos += new Vector3(3, 0, 0);
        controlDevices.Add(ControlDevice.Gamepad);
    }
}
