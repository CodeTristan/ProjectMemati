using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BombaciControl bombaci = other.GetComponent<BombaciControl>();
            if (bombaci != null)
            {
                bombaci.TakeBomb(transform);
                Debug.Log("AAAAAAAAA");
            }
        }
    }
}
