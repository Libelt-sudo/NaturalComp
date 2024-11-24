using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Random = UnityEngine.Random;


public class BoidManager : MonoBehaviour
{
    [SerializeField] private GameObject boidPrefab;
    public HashSet<GameObject> neighbours = new HashSet<GameObject>();
    [SerializeField] private int boidCount = 50;
    [SerializeField] private Vector3 spawnArea = new Vector3(70f, 0f, 70f);
    /*[SerializeField] private Vector3 spawnArea2 = new Vector3(200f, 0f, 200f);*/

    private void Start()
    {
        for (int i = 0; i < boidCount; i++)
        {
            Instantiate(boidPrefab, new Vector3(Random.Range(-spawnArea.x, spawnArea.x),1f,Random.Range(-spawnArea.z, spawnArea.z)), Quaternion.identity );
            
        }

        /*for (int i = 0; i < boidCount; i++)
        {
            Instantiate(boidPrefab, new Vector3(Random.Range(150f, spawnArea2.x),1f,Random.Range(150f, spawnArea2.z)), Quaternion.identity );
        }*/
    }
}