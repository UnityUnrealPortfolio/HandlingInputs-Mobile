using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Debug related properties")]
    [SerializeField]TMP_Text debugText;
    [Tooltip("Balls automatic move forward speed")]
    [Range(0f, 10f)]
    [SerializeField] float ballSpeed;

    [Tooltip("how fast the ball moves left or right")]
    [Range(0f,10f)]
    [SerializeField] float ballDodgeSpeed;

    [Header("Swipe related properties")]
    [Tooltip("length of the swipe distance on the screen beyond which we register a swipe")]
    [SerializeField]float minSwipeDistanceInInches = 0.25f;

    [Tooltip("How far the player moves in response to the swipe")]
    [SerializeField] float playerSwipeMoveDistance = 2f;
    float minSwipeDistanceInPixels;
    Vector2 startTouchPos;
    Vector2 endTouchPos;

    [Header("Scale Related Properties")]
    [Tooltip("Minimum scale the player can be")]
    [SerializeField] float minPlayerScale = 0.5f;

    [Tooltip("Maximum scale the player can be")]
    [SerializeField] float maxPlayerScale = 3f;

    float currentScale = 1;


    /// <summary>
    /// A reference to the Rigidbody component
    /// </summary>
    Rigidbody playerRB;

    /// <summary>
    /// a reference to the horizontal speed of player in response to inputs
    /// </summary>
    float horizontalSpeed;

    private void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        minSwipeDistanceInPixels = minSwipeDistanceInInches * Screen.dpi;
    }

    private void Update()
    {
        float xMove = 0;
#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBPLAYER
        
        //Handling tap
        //convert tap position from screen(pixels) to view port coordinate
        MoveWithTap(ref xMove);

#elif UNITY_ANDROID || UNITY_IOS
        
        //Handling touch
        //MoveWithTap(ref xMove);
        MoveWithSwipe();
        ScaleWithPinch();
      
#endif
        horizontalSpeed = ballDodgeSpeed * xMove;
       
    }


    private void FixedUpdate()
    {
        playerRB.AddForce(horizontalSpeed, 0f, ballSpeed);
    }
    private void MoveWithTap(ref float xMove)
    {
        if (Input.GetMouseButton(0))
        {
            //are we on left or right of the view port 

            var viewportPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);


            if (viewportPosition.x > 0.5f)
            {
                //we are to the right
                //set xMove to a positive value
                xMove = 1;
            }
            if (viewportPosition.x < 0.5)
            {
                //we are to the left
                //set xMove to a negative value
                xMove = -1;
            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("inside else if MoveWithTap()");
            xMove = 0f;
        }

      
    }

    private void MoveWithTouch(ref float xMove)
    {
        //has there been a touch on the screen
        if(Input.touchCount > 0)
        {
            //get the first touch
            var touch = Input.touches[0];

            //convert screen coordinates to viewport coordinates for easer working
            var viewportPosition = Camera.main.ScreenToViewportPoint(touch.position);


            //determine if right or left of screen was tapped respectively
            if (viewportPosition.x > 0.5f)
            {
                //we are to the right
                //set xMove to a positive value
                xMove = 1;
            }
            if (viewportPosition.x < 0.5)
            {
                //we are to the left
                //set xMove to a negative value
                xMove = -1;
            }
        }
    }

    private void MoveWithSwipe()
    {
        //are we touching the screen
        if(Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            if(touch.phase == TouchPhase.Began)
            {
                startTouchPos = touch.position;
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                endTouchPos = touch.position;
                CalculateSwipeDistance();
            }

        }
    }

    private void ScaleWithPinch()
    {
        //check if two fingers are on the screen
        if(Input.touchCount != 2)
        {
            return;
        }
        else
        {
            //store each of these touches
            Touch touchOne = Input.touches[0];
            Touch touchTwo = Input.touches[1];

            Vector2 touchOnePrev = touchOne.position - touchOne.deltaPosition;
            Vector2 touchTwoPrev = touchTwo.position - touchTwo.deltaPosition;

            //distance between the two touches in each frame
            float prevTouchDeltaMag = (touchOnePrev - touchTwoPrev).magnitude;
            float touchDeltaMag = (touchOne.position - touchTwo.position).magnitude;

            //difference in distance in each frame
            float deltaMagDiff = prevTouchDeltaMag - touchDeltaMag;
           
            float newScale = currentScale - (deltaMagDiff * Time.deltaTime);
            newScale = Mathf.Clamp(newScale, minPlayerScale, maxPlayerScale);

            //update the player's scale
            transform.localScale = Vector3.one * newScale;

            currentScale = newScale;
        }

    }
    private void CalculateSwipeDistance()
    {
        //what's the distance between the swipes in pixels?
        float swipeDiff = endTouchPos.x - startTouchPos.x;
        float swipeDiffInX = Mathf.Abs(swipeDiff);
       
        //does swipe distance meet our threshold for registration?
        if(swipeDiffInX < minSwipeDistanceInPixels)
        {
            return;
        }

        Vector3 moveDirection = Vector3.zero;

        if(swipeDiffInX > minSwipeDistanceInPixels)
        {
            //moving positively in the x
           if(swipeDiff > 0)
            {
                moveDirection = Vector3.right;
            }
            else
            {
                moveDirection = Vector3.left;
            }
        }

        //check to see if our movement will collide will anything. if so, don't swipe
        RaycastHit hitInfo;

        if(!playerRB.SweepTest(moveDirection, out hitInfo,playerSwipeMoveDistance))
        {
            playerRB.MovePosition(playerRB.position + moveDirection * playerSwipeMoveDistance);
        }
    }
}
