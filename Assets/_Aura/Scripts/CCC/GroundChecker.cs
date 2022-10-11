using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    //check for ground using a sphere cast
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float sphereCastDistance;
    SphereCollider ballCollider;
    [field:SerializeField]public bool isGrounded { get; set; }

    private void Start()
    {
        ballCollider = GetComponent<SphereCollider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
