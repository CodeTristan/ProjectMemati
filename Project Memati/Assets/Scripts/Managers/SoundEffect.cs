using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public float time;
    public AudioSource source;

    private void Start()
    {
        Invoke("kill",time);
    }

    public void kill()
    {
        Destroy(gameObject);
    }
}
