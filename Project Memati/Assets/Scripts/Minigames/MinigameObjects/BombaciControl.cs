using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BombaciControl : PlayerControl
{
    public int sikor = 0;
    public Camera playerCamera;

    public Vector3 offset = new Vector3(0, 10, -10);

    public TextMeshProUGUI scoreText;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        playerCamera = Camera.main;
    }

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


    protected float AddCameraRotationToAngle(float angle)
    {
        angle += playerCamera.transform.eulerAngles.y;
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


    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }


    }

    protected void OnTriggerEnter(Collider other)
    {
    }

    protected void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
            jumpInput = 0;
        }
    }
}
