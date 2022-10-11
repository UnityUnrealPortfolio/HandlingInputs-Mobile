using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelModuleBehaviour : MonoBehaviour
{
    [SerializeField] Transform[] obstaclePositions;

    [SerializeField] GameObject obstaclePrefab;

    public void InitObstacles()
    {
        int randomIndex = Random.Range(0, obstaclePositions.Length);
        
        //instantiate an obstacle
        var obstacle = Instantiate(obstaclePrefab);
        obstacle.transform.position = obstaclePositions[randomIndex].position;
    }
}
