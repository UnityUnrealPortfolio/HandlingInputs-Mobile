using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //check if player has entered trigger
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.ResetGame();
        }
    }
}
