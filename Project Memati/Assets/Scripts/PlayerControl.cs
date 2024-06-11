using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody rb;
    public Player player;
    public float speed;
    public float jumpPower;
    public bool isGrounded;

    public InputDevice device;

    public Transform MainCameraTransform;
    public Vector3 dampedTargetRotationPassedTime;
    public Vector3 dampedTargetRotationCurrentVelocity;
    public Vector3 dampedTargetRotationPassedPassedTime;
    public Vector3 timeToReachTargetRotation;

    protected Vector2 moveInput;
    protected float jumpInput;
    protected PlayerActions inputActions;
    protected Vector3 currentTargetRotation;

    public enum ControlDevice
    {
        KeyboardLeft,
        KeyboardRight,
        Gamepad
    }

    //Control device türüne göre kontrolleri açar.
    public void Init(ControlDevice controlDevice,InputDevice device)
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new PlayerActions();
        MainCameraTransform = Camera.main.transform;
        this.device = device;
        if (controlDevice == ControlDevice.KeyboardLeft)
        {
            inputActions.KeyboardLeft.Enable();
            inputActions.KeyboardLeft.Move.Enable();
            inputActions.KeyboardLeft.Move.performed += OnMove;
            inputActions.KeyboardLeft.Jump.performed += OnJump;
        }
        else if (controlDevice == ControlDevice.KeyboardRight)
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

    //Collision olursa bu kod içerisinde kontrol edilmesi lazým. Collision'a giriþte burasý çalýþýr.
    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }


    //Collision olursa bu kod içerisinde kontrol edilmesi lazým. Collision'dan çýkýþta burasý çalýþýr.
    protected void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
            jumpInput = 0;
        }
    }

    //Hýzý ayarlama.
    protected void FixedUpdate()
    {
        if (moveInput == Vector2.zero || speed == 0f)
        {
            rb.velocity = new Vector3(0, 0, 0);
            return;
        }
        Vector3 MovementDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        float targetRotationYAngle = Rotate(MovementDirection);
        Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

        Debug.Log(targetRotationDirection.x);
        rb.velocity = new Vector3(targetRotationDirection.x * speed, rb.velocity.y + jumpInput, targetRotationDirection.z * speed);
        Debug.Log(moveInput + "vector2");
        Debug.Log(targetRotationDirection.z + "vector3");
    }

    //Input system event'ine subscribe olmak için yazýlan kod. Aygýttan gelen deðeri okuyup moveInput'a atar.
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.control.device.deviceId == device.deviceId)
            moveInput = context.ReadValue<Vector2>();
    }

    //Input system event'ine subscribe olmak için yazýlan kod. Eðer ki karakter yerdeyse zýplatýlýr.
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!isGrounded) return;

        if (context.control.device.deviceId == device.deviceId)
            jumpInput = jumpPower;
    }

    //Debug mesaj atmak için
    public void DebugMsg(string msg)
    {
        Debug.Log(msg);
    }

    protected float AddCameraRotationToAngle(float angle)
    {
        angle += MainCameraTransform.eulerAngles.y;
        if (angle > 360)
        {
            angle -= 360f;
        }

        return angle;
    }

    protected void UpdateTargetRotationData(float targetAngle)
    {
        currentTargetRotation.y = targetAngle;
        dampedTargetRotationPassedPassedTime.y = 0f;
    }

    protected float UpdateTargetRotation(Vector3 direction)
    {
        float directionAngle = GetDirectionAngle(direction);

        directionAngle = AddCameraRotationToAngle(directionAngle);

        if (directionAngle != currentTargetRotation.y)
        {
            UpdateTargetRotationData(directionAngle);
        }

        return directionAngle;
    }

    protected static float GetDirectionAngle(Vector3 direction)
    {
        float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        if (directionAngle < 0f)
        {
            directionAngle += 360;
        }

        return directionAngle;
    }

    protected void RotateTowardsTargetRotation()
    {
        float currentYAngle = rb.rotation.eulerAngles.y;

        if (currentYAngle == currentTargetRotation.y)
        {
            return;
        }

        float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, currentTargetRotation.y, ref dampedTargetRotationCurrentVelocity.y, 0.14f - dampedTargetRotationPassedTime.y);
        dampedTargetRotationPassedTime.y += Time.deltaTime;

        Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);
        rb.MoveRotation(targetRotation);
    }

    protected float Rotate(Vector3 direction)
    {
        float directionAngle = UpdateTargetRotation(direction);
        RotateTowardsTargetRotation();

        return directionAngle;
    }

    protected Vector3 GetTargetRotationDirection(float targetAngle)
    {
        return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    }
}
