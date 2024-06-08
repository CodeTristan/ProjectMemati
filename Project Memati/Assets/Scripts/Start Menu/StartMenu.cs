using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void StartGame()

    {
        SceneManager.LoadScene(1); // Loading Lobby Scene
    }

    public void QuitGame()
    {
        Debug.Log("Quit button clicked.");
        Application.Quit();
    }
}
