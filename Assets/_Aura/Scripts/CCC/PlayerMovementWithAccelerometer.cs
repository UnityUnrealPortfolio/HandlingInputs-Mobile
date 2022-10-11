using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerMovementWithAccelerometer : MonoBehaviour
{
    Rigidbody playerRB;
    [SerializeField] Transform cameraTransform;
    [SerializeField] float forwardTiltAdjust = 7f;
    [SerializeField] float forwardTiltCutoff = -0.6f;
    [SerializeField]TMP_Text debugText;
    [SerializeField]float moveForce;

    Vector3 moveInput = Vector3.zero;
    Vector3 heightMovement = Vector3.zero;
    Vector3 moveDirection = Vector3.zero;

    private void Start()
    {
        playerRB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float zMove = 0f;
       
        float forwardBackMovement = Input.acceleration.y;
       
        if(forwardBackMovement > -forwardTiltCutoff)
        {
            forwardBackMovement = 0f;
            zMove = 0;
        }
        else
        {
            
            forwardBackMovement = Mathf.Abs(Input.acceleration.y);
            

            if (forwardBackMovement > forwardTiltAdjust)
            {
              
                zMove = -1f;
            }
            else if(forwardBackMovement < forwardTiltAdjust)
            {
               
                zMove = 1f;
            }
        }
        

       
        moveInput.z =  zMove * moveForce;
       
    }


    private void FixedUpdate()
    {
        Vector3 forwardMovement = Camera.main.transform.forward * moveInput.z;

        moveDirection = forwardMovement + heightMovement;
        playerRB.velocity = moveDirection;
    }
}


