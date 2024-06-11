using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    public GameObject characterModel;
    public GameObject player;
    void OnTriggerEnter(Collider other) {
        //this.gameObject.GetComponent<Collider>().enabled = false;
        player.GetComponent<PlayerMove>().enabled = false;
        characterModel.GetComponent<Animator>().Play("Falling Down");
    }
}
