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

    //Control device t�r�ne g�re kontrolleri a�ar.
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

    //Collision olursa bu kod i�erisinde kontrol edilmesi laz�m. Collision'a giri�te buras� �al���r.
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }


    //Collision olursa bu kod i�erisinde kontrol edilmesi laz�m. Collision'dan ��k��ta buras� �al���r.
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
            jumpInput = 0;
        }
    }

    //H�z� ayarlama.
    private void FixedUpdate()
    {
        rb.velocity = new Vector3(moveInput.x * speed, rb.velocity.y + jumpInput,moveInput.y * speed);
    }

    //Input system event'ine subscribe olmak i�in yaz�lan kod. Ayg�ttan gelen de�eri okuyup moveInput'a atar.
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    //Input system event'ine subscribe olmak i�in yaz�lan kod. E�er ki karakter yerdeyse z�plat�l�r.
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!isGrounded) return;

        jumpInput = jumpPower;
    }

    //Debug mesaj atmak i�in
    public void DebugMsg(string msg)
    {
        Debug.Log(msg);
    }
}
