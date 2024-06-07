using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SahneManager : MonoBehaviour
{
    public static SahneManager instance;

    public void Init()
    {
        instance = this;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //Sahne yüklendiði an bu fonksiyon otomatik olarak çaðýrýlýr. Sahne yüklenince bir þeyler yapýlmasýný istiyorsanýz bunun içine yazýn.
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        
    }

    //Ýsmi verilen sahneyi yükler
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
