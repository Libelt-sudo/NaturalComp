using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CrowdBehaviour : ScriptableObject
{
    public abstract Vector3 CalculateMove(CrowdAgent agent, List<Transform> context, Crowd crowd);
    
    


}
