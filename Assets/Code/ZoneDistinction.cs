using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneDistinction : MonoBehaviour
{
    public PlayerManager.ZoneLevel thisZone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerManager>().currentZone = thisZone;
            other.GetComponent<PlayerManager>().SpawnFish();
        }
            
    }
}
