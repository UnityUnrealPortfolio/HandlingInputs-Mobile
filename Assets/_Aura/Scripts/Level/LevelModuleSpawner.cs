using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelModuleSpawner : MonoBehaviour
{
    [Tooltip("Prefab of a level module to spawn")]
    [SerializeField] GameObject levelModule;

    [Tooltip("Dimensions of a module in z")]
    [SerializeField]float levelModuleDimensionInZ;

    [Tooltip("How many tiles should we create in advance")]
    [Range(1,20)]
    [SerializeField] int initModulesToSpawn;


    float spawnerPosInZ;
    private void Start()
    {
        spawnerPosInZ = transform.position.z;
        SpawnInitialModules();
        InvokeRepeating("SpawnNextModule", .1f,1f);
    }

    private void SpawnInitialModules()
    {
        for(int i = 0; i < initModulesToSpawn; i++)
        {
            var nextSpawnPosInZ = spawnerPosInZ + levelModuleDimensionInZ;
            var module = Instantiate(levelModule);
            module.transform.position = new Vector3(transform.position.x, transform.position.y, nextSpawnPosInZ);
            spawnerPosInZ = nextSpawnPosInZ;
        }
      
    }
    private void SpawnNextModule()
    {
        var nextSpawnPosInZ = spawnerPosInZ + levelModuleDimensionInZ;
        var module = Instantiate(levelModule);
        module.transform.position = new Vector3(transform.position.x, transform.position.y, nextSpawnPosInZ);
        spawnerPosInZ = nextSpawnPosInZ;
        module.GetComponent<LevelModuleBehaviour>().InitObstacles();
    }
}
