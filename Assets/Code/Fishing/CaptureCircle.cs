using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureCircle : MonoBehaviour
{
    [HideInInspector] public bool fishInCircle;

    [HideInInspector] public float sizeUpgrade = 1f;

    private float baseSize = 3;
    public Transform targetTransform;
    private float baseTargetSize = 0.5f;
    public void ChangeCatcherSize()
    {
        transform.localScale = new Vector3(baseSize * sizeUpgrade, baseSize * sizeUpgrade, baseSize * sizeUpgrade);
        targetTransform.localScale = new Vector3(baseTargetSize * sizeUpgrade, baseTargetSize * sizeUpgrade, baseTargetSize * sizeUpgrade);
    }
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
