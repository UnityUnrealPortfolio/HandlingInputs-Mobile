using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerBallCameraBehaviour : MonoBehaviour
{
    //target the player
    //set heightOffset above player
    //set distance in Z behind player

    [SerializeField] float heightOffset;
    [SerializeField] float zOffsetBehindPlayer;
    [SerializeField] float rotationSensitivity;
    GameObject player;
    Vector3 cameraTarget;
    float horizontalMovement;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        cameraTarget = player.transform.position;
        cameraTarget.y += heightOffset;

        horizontalMovement += Input.acceleration.x * rotationSensitivity;
    }

    private void LateUpdate()
    {
        Vector3 camOffset = new Vector3(0, 0, -zOffsetBehindPlayer);

        Quaternion camRotation = Quaternion.Euler(0,horizontalMovement,0);
        transform.position = cameraTarget + camRotation * camOffset;

        transform.LookAt(cameraTarget);
    }
}
