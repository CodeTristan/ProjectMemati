using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField] float moveSpeed = 9;
    //[SerializeField] float leftRightSpeed = 6;
    Vector3 moveDirection;

    public InputActionReference move;

    void FixedUpdate() {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, 0);
    }
    void Update()
    {
        transform.Translate(0, 0, moveSpeed * Time.deltaTime);

        moveDirection = move.action.ReadValue<Vector2>();

        
/*         if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
            if(this.gameObject.transform.position.x > LevelBoundary.leftSide){
                transform.Translate(Vector3.left * leftRightSpeed * Time.deltaTime);
            }
        }

        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
            if(this.gameObject.transform.position.x < LevelBoundary.rightSide){
                transform.Translate(Vector3.right * leftRightSpeed * Time.deltaTime);
            }
        } */


        
    }
}
