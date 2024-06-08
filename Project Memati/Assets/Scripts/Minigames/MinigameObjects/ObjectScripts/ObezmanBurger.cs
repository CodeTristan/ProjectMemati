using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObezmanBurger : MonoBehaviour
{
    public int amkskoru = 10;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ScoreManager.instance.AddScore(amkskoru);

            Destroy(gameObject);
        }
    }
}
