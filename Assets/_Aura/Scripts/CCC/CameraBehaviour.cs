using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [Tooltip("What object should the camera be looking at")]
    public Transform target;

    [Tooltip("How offset will the camera be to the target")]
    public Vector3 offset = new Vector3(0, 3, -6);

    private void Update()
    {
        Debug.Log(target.transform.forward);
        //is there a target
        if(target != null)
        {
            //set position of camera to an offset of our target
            transform.position = target.position + offset;

            //set rotation of camera to look at the target
            transform.LookAt(target);
        }
    }
}
