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
            if(gameObject.transform.parent != null)
            {
                BombaciControl bombaci2 = gameObject.transform.parent.GetComponent<BombaciControl>();
                bombaci2.hasbomb = false;
            }
            
            if (bombaci != null)
            {
                bombaci.TakeBomb(transform);
            }
        }
    }
}
