using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField] float moveSpeed = 9;
    Vector3 moveDirection;
    public InputActionReference move;

    void Update()
    {
        transform.Translate(0, 0, moveSpeed * Time.deltaTime);
        moveDirection = move.action.ReadValue<Vector2>();
    }

    void FixedUpdate() {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, 0);
    }

}
