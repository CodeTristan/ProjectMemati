using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObezmanBurger : MonoBehaviour
{
    public int amkskoru = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>(); 

            if (player != null) // Oyuncu bileşenini başarıyla aldıysanız
            {
                ScoreManager.instance.AddScore(player, amkskoru); // Skoru ekleyin
            }

            Destroy(gameObject); // Hamburger objesini yok edin
        }
    }
}
