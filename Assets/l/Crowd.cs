using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Random = UnityEngine.Random;

public class Crowd : MonoBehaviour
{
    public CrowdAgent agentPrefab;
    private List<CrowdAgent> agents = new List<CrowdAgent>();
    public CrowdBehaviour behaviour;

    [Range(10, 500)] public int initCrowdSize = 250;
    private const float agentDensity = 0.08f;

    [Range(1.0f, 100f)] public float driveFactor = 10f;
    [Range(1.0f, 100f)] public float maxSpeed = 5.0f;
    [Range(1.0f, 10f)] public float neighbourRadius = 1.5f;
    [Range(1.0f, 100f)] public float avoidanceRadiusMultiplier = 5.0f;

    private float squareMaxSpeed;
    private float squareNeighbourRadius;
    private float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }


    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighbourRadius = neighbourRadius * neighbourRadius;
        squareAvoidanceRadius = squareNeighbourRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < initCrowdSize; i++)
        {
            CrowdAgent newAgent = Instantiate(
                agentPrefab,
                new Vector3(
                    Random.insideUnitCircle.x * initCrowdSize * agentDensity, 
                    2f, 
                    Random.insideUnitCircle.y * initCrowdSize * agentDensity
                    ),
                Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)),
                transform
                );
            newAgent.name = "Agent " + i;
            agents.Add(newAgent);
        }

    }
    

    // Update is called once per frame
    void Update()
    {
        foreach (CrowdAgent agent in agents)
        {
            List<Transform> context = GetNearByObjects(agent);
            
            
            agent.GetComponentInChildren<Renderer>().material.color = Color.Lerp(Color.green, Color.red, context.Count / 3f) ;
            
            Vector3 move = behaviour.CalculateMove(agent, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);
        }
    }

    private List<Transform> GetNearByObjects(CrowdAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighbourRadius);
        foreach (Collider c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }

        return context;
    }

}
