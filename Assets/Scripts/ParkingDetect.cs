using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingDetect : MonoBehaviour
{
    /// <summary>
    /// The associated agent.
    /// This will be set by the agent script on Initialization.
    /// Don't need to manually set.
    /// </summary>
    
    public CarAgent agent;  //

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("parking"))
        {
            agent.parked();
        }
    }
    
}
