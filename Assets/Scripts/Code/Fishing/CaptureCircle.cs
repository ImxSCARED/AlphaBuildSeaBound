using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureCircle : MonoBehaviour
{
    public bool fishInCircle;

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Fish")
        {
            fishInCircle = true;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if(collision.tag == "Fish")
        {
            fishInCircle = false;
        }
    }
}
