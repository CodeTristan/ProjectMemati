using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    public int playerId;
    public float speed;
    public float jumpPower;
    public bool isGrounded;

    private Vector2 moveInput;
    private float jumpInput;
    private PlayerActions inputActions;

    public enum ControlDevice
    {
        KeyboardLeft,
        KeyboardRight,
        Gamepad
    }
    public void Init(ControlDevice controlDevice)
    {
        inputActions = new PlayerActions();

        if(controlDevice == ControlDevice.KeyboardLeft)
        {
            inputActions.KeyboardLeft.Enable();
            inputActions.KeyboardLeft.Move.Enable();
            inputActions.KeyboardLeft.Move.performed += OnMove;
            inputActions.KeyboardLeft.Jump.performed += OnJump;
        }
        else if(controlDevice == ControlDevice.KeyboardRight)
        {
            inputActions.KeyboardRight.Enable();
            inputActions.KeyboardRight.Move.Enable();
            inputActions.KeyboardRight.Move.performed += OnMove;
            inputActions.KeyboardRight.Jump.performed += OnJump;

        }
        else
        {
            inputActions.Gamepad.Enable();
            inputActions.Gamepad.Move.Enable();
            inputActions.Gamepad.Move.performed += OnMove;
            inputActions.Gamepad.Jump.performed += OnJump;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
            jumpInput = 0;
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector3(moveInput.x * speed, rb.velocity.y + jumpInput,moveInput.y * speed);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!isGrounded) return;

        jumpInput = jumpPower;
    }

    public void DebugMsg(string msg)
    {
        Debug.Log(msg);
    }
}
