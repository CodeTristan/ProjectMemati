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

    public Transform MainCameraTransform;
    public Vector3 dampedTargetRotationPassedTime;
    public Vector3 dampedTargetRotationCurrentVelocity;
    public Vector3 dampedTargetRotationPassedPassedTime;
    public Vector3 timeToReachTargetRotation;

    private Vector2 moveInput;
    private float jumpInput;
    private PlayerActions inputActions;
    private Vector3 currentTargetRotation;

    public enum ControlDevice
    {
        KeyboardLeft,
        KeyboardRight,
        Gamepad
    }

    //Control device türüne göre kontrolleri açar.
    public void Init(ControlDevice controlDevice)
    {
        inputActions = new PlayerActions();

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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }


    //Collision olursa bu kod içerisinde kontrol edilmesi lazým. Collision'dan çýkýþta burasý çalýþýr.
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
            jumpInput = 0;
        }
    }

    //Hýzý ayarlama.
    private void FixedUpdate()
    {
        Vector3 MovementDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        float targetRotationYAngle = Rotate(MovementDirection);
        Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

        Debug.Log(targetRotationDirection.x);
        rb.velocity = new Vector3(moveInput.x * speed*targetRotationDirection.x, rb.velocity.y + jumpInput, moveInput.y * speed*targetRotationDirection.y);
    }

    //Input system event'ine subscribe olmak için yazýlan kod. Aygýttan gelen deðeri okuyup moveInput'a atar.
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    //Input system event'ine subscribe olmak için yazýlan kod. Eðer ki karakter yerdeyse zýplatýlýr.
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!isGrounded) return;

        jumpInput = jumpPower;
    }

    //Debug mesaj atmak için
    public void DebugMsg(string msg)
    {
        Debug.Log(msg);
    }

    private float AddCameraRotationToAngle(float angle)
    {
        angle += MainCameraTransform.eulerAngles.y;
        if (angle > 360)
        {
            angle -= 360f;
        }

        return angle;
    }

    private void UpdateTargetRotationData(float targetAngle)
    {
        currentTargetRotation.y = targetAngle;
        dampedTargetRotationPassedPassedTime.y = 0f;
    }

    private float UpdateTargetRotation(Vector3 direction)
    {
        float directionAngle = GetDirectionAngle(direction);

        directionAngle = AddCameraRotationToAngle(directionAngle);

        if (directionAngle != currentTargetRotation.y)
        {
            UpdateTargetRotationData(directionAngle);
        }

        return directionAngle;
    }

    private static float GetDirectionAngle(Vector3 direction)
    {
        float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        if (directionAngle < 0f)
        {
            directionAngle += 360;
        }

        return directionAngle;
    }

    private void RotateTowardsTargetRotation()
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

    private float Rotate(Vector3 direction)
    {
        float directionAngle = UpdateTargetRotation(direction);
        RotateTowardsTargetRotation();

        return directionAngle;
    }

    private Vector3 GetTargetRotationDirection(float targetAngle)
    {
        return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    }
}
