/*
 *      Third Person Player Movement Script v1.1 by Ian McCambridge
 *      :: Free to use always <3 2020 ::
 *
*       This script pairs with my "Third Person Camera Script" which can be found here:
*       https://gist.github.com/kidchemical/b1542ea489c8f2abae3fbd09798dedd4
 *   FEATURE OUTLINE:
     *   -Rigidbody required.
     *      -Plane or Ground must have Tag property set to new tag named "Ground"
     *      -Freeze X and Z Rotation for player Rigidbody
     *      -Uses 'force' for movement, but 'transform ' for rotation.
     *   -WASD Movement, Spacebar Jump, Controller support
     *   -No Strafe, horizontal axis of input turns (rotates) player (feels like driving controls...)
 *
 *
 *   TO ADD:
     *   -Loose Camera "Look At"
     *   -Camera snaps behind player when moving / running?
     *   -Fix turning mechanism to feel more natural
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    float moveSpeed = 2;
    float rotationSpeed = 4;
    float runningSpeed;
    float vaxis, haxis;
    public bool isJumping, isJumpingAlt, isGrounded = false;
    Vector3 movement;

    void Start()
    {
        Debug.Log("Initialized: (" + this.name + ")");
    }


    void FixedUpdate()
    {
        /*  Controller Mappings */
        vaxis = Input.GetAxis("Vertical");
        haxis = Input.GetAxis("Horizontal");
        isJumping = Input.GetButton("Jump");
        isJumpingAlt = Input.GetKey(KeyCode.Joystick1Button0);

        //Simplified...
        runningSpeed = vaxis;


        if (isGrounded)
        {
            movement = new Vector3(0, 0f, runningSpeed * 8);        // Multiplier of 8 seems to work well with Rigidbody Mass of 1.
            movement = transform.TransformDirection(movement);      // transform correction A.K.A. "Move the way we are facing"
        }
        else
        {
            movement *= 0.70f;                                      // Dampen the movement vector while mid-air
        }

        GetComponent<Rigidbody>().AddForce(movement * moveSpeed);   // Movement Force


        if ((isJumping || isJumpingAlt) && isGrounded)
        {
            Debug.Log(this.ToString() + " isJumping = " + isJumping);
            GetComponent<Rigidbody>().AddForce(Vector3.up * 150);
        }



        if ((Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f) && !isJumping && isGrounded)
        {
            if (Input.GetAxis("Vertical") >= 0)
                transform.Rotate(new Vector3(0, haxis * rotationSpeed, 0));
            else
                transform.Rotate(new Vector3(0, -haxis * rotationSpeed, 0));

        }

    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Entered");
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log("Exited");
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
