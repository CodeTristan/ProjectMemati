using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : PlayerControl
{
    public float forwardSpeed;
    public bool startMove;
    private void FixedUpdate() 
    {
        if (startMove)
            rb.velocity = new Vector3(moveInput.x * speed, rb.velocity.y + jumpInput, forwardSpeed);
    }
    //Collision olursa bu kod i�erisinde kontrol edilmesi laz�m. Collision'a giri�te buras� �al���r.
    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
        if(collision.gameObject.tag == "TempleRunBoundary")
        {
            //S�n�rlara �arparsa sa� sol h�z� 0 oluyo sadece ileri gitsin diye. Yoksa kenarlara tak�l�r.
            moveInput = Vector2.zero;
        }
        if (collision.gameObject.tag == "TempleRunObstacle")
        {
            //KARAKTER BURADA �L�YOR. �lme animasyonunu buraya eklemelisin.
            Debug.Log("You are dead");
            forwardSpeed = 0;
            moveInput = Vector2.zero;
            GenerateLevel.instance.playerCount--;
        }

        if(collision.gameObject.tag == "FinishLine"){
            GenerateLevel.instance.winners.Add(player);
            GenerateLevel.instance.playerCount--;
        }
    }


    //Collision olursa bu kod i�erisinde kontrol edilmesi laz�m. Collision'dan ��k��ta buras� �al���r.
    protected void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
            jumpInput = 0;
        }
    }
    //Input system event'ine subscribe olmak i�in yaz�lan kod. Ayg�ttan gelen de�eri okuyup moveInput'a atar.
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.control.device.deviceId == device.deviceId)
            moveInput = context.ReadValue<Vector2>();
    }

    //Input system event'ine subscribe olmak i�in yaz�lan kod. E�er ki karakter yerdeyse z�plat�l�r.
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!isGrounded) return;

        if (context.control.device.deviceId == device.deviceId){
            jumpInput = jumpPower;
        }
            
    }
}
