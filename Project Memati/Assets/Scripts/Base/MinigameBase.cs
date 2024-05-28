using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MinigameBase : MonoBehaviour
{
    public string MinigameName;
    public string SceneName;

    public abstract void Init();

}
