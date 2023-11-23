using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiFishingHitbox : MonoBehaviour
{
    public bool fishInZone = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fish"))
        {
            fishInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Fish"))
        {
            fishInZone = false;
        }
    }
}
