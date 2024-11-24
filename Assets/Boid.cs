using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Random = UnityEngine.Random;


public class Boid : MonoBehaviour
{
    public float minSpeed = 5f;
    public float maxSpeed = 20f;
    
    private Vector3 velocity;

    private float protectedRange = 5f;
    private float visualRange = 15f;
    private float visualRangeSquared;

    private float alignmentFactor = 0.5f;
    private float centeringFactor = 0.005f;
    private float seperationFactor = 0.5f;

    public HashSet<GameObject> neighbours = new HashSet<GameObject>();
    public LayerMask boidLayerMask;

    public SphereCollider boidSphereCollider;

    private RaycastHit hit;

    private void Start()
    {
        boidSphereCollider = GetComponent<SphereCollider>();
        boidSphereCollider.radius = visualRange;
        visualRangeSquared = visualRange * visualRange;
    }

    private void Update()
    {
        
        transform.position += velocity * Time.deltaTime;

        // Optionally: Apply random small adjustments to direction for erratic behavior
        Vector3 randomAdjustment = new Vector3(
            Random.Range(-0.5f, 0.5f),
            velocity.y,
            Random.Range(-0.5f, 0.5f)
        );

        velocity += randomAdjustment;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed); // Ensure velocity stays within limits

        // Update boid's forward direction
        transform.forward = velocity.normalized;
        alignment();
        Cohesion();
        Seperation();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger && other.gameObject.layer == LayerMask.NameToLayer("Boid"))
        {
            // Check if the object is within visual range
            float sqDistanceToNeighbour = (other.transform.position - transform.position).sqrMagnitude;
            if (sqDistanceToNeighbour < visualRangeSquared)
            {
                neighbours.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger && other.gameObject.layer == LayerMask.NameToLayer("Boid"))
        {
            // Remove the neighbor when exiting the range
            neighbours.Remove(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.isTrigger && other.gameObject.layer == LayerMask.NameToLayer("Boid"))
        {
            // Check distance only if the object is not already in the list
            float sqDistanceToNeighbour = (other.transform.position - transform.position).sqrMagnitude;

            if (sqDistanceToNeighbour < visualRangeSquared)
            {
                neighbours.Add(other.gameObject);
            }
            else if (sqDistanceToNeighbour > visualRangeSquared)
            {
                neighbours.Remove(other.gameObject);
            }
        }
    }

    public void alignment()
    {
        Vector3 velocityAvg = Vector3.zero;
        int neighbourCount = 0;
        

        foreach (GameObject neighbouringBoid in neighbours)
        {
            velocityAvg += neighbouringBoid.GetComponent<Boid>().velocity;
            neighbourCount++;
        }

        if (neighbourCount > 0)
        {
            velocityAvg /= neighbourCount;
            velocity += (velocityAvg - velocity) * alignmentFactor;
            velocity.y = 0f;
        }

    }

    public void Cohesion()
    {
        Vector3 positionAvg = Vector3.zero;
        int neighbourCount = 0;
        
        foreach (GameObject neighbouringBoid in neighbours)
        {
            positionAvg += neighbouringBoid.GetComponent<Boid>().transform.position;
            neighbourCount++;
        }
        
        if (neighbourCount > 0)
        {
            positionAvg /= neighbourCount;
            Vector3 directionToCenter = positionAvg - transform.position;
            velocity += directionToCenter.normalized * centeringFactor;
            velocity.y = 0f;
        }
        
    }

    public void Seperation()
    {
        Vector3 closeDifference = Vector3.zero;

        foreach (GameObject neighbouringBoid in neighbours)
        {
            Vector3 otherBoidPosition = neighbouringBoid.GetComponent<Boid>().transform.position;
            float sqDistanceToNeighbour = (otherBoidPosition - transform.position).sqrMagnitude;

            if (sqDistanceToNeighbour < protectedRange)
            {
                closeDifference += transform.position - otherBoidPosition;
            }
        }

        velocity += closeDifference * seperationFactor;
        velocity.y = 0f;

    }


}