using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureCircle : MonoBehaviour
{
    public bool fishInCircle;
    public float sizeUpgrade = 1f;
    private Vector2 baseSize = new Vector2(3, 3);
    public void ChangeCatcherSize()
    {
        transform.localScale = new Vector3(baseSize.x * sizeUpgrade, 0.01f, baseSize.y * sizeUpgrade);
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
