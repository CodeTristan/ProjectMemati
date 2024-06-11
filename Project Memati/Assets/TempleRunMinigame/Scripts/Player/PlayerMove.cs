using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : PlayerControl
{
    public float forwardSpeed;

    private void FixedUpdate() 
    {
        rb.velocity = new Vector3(moveInput.x * speed, rb.velocity.y + jumpInput, forwardSpeed);
    }
    //Collision olursa bu kod içerisinde kontrol edilmesi lazým. Collision'a giriþte burasý çalýþýr.
    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
        if(collision.gameObject.tag == "TempleRunBoundary")
        {
            //Sýnýrlara çarparsa sað sol hýzý 0 oluyo sadece ileri gitsin diye. Yoksa kenarlara takýlýr.
            moveInput = Vector2.zero;
        }
        if (collision.gameObject.tag == "TempleRunObstacle")
        {
            //KARAKTER BURADA ÖLÜYOR. Ölme animasyonunu buraya eklemelisin.
            Debug.Log("You are dead");
            forwardSpeed = 0;
            moveInput = Vector2.zero;
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
}
