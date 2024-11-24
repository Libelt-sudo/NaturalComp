using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;
using Vector3 = UnityEngine.Vector3;

/*[CreateAssetMenu(menuName = "Crowd/Behaviour/Cohesion")]*/
/*public class CohesionBehaviour : CrowdBehaviour
{
    public override Vector3 CalculateMove(CrowdAgent agent, List<Transform> context, Crowd crowd)
    {
        if (context.Count == 0)
        {
            return Vector3.zero;
        }
        
        Vector3 cohesionMove = Vector3.zero;

        foreach (Transform item in context)
        {
            cohesionMove += item.position;
        }

        cohesionMove /= context.Count;

}    }*/
