using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogReverse : MonoBehaviour
{

    private GameObject boatRotation;

    // Add reference for camera script. Rotating the Y value will auto correct camera position.


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("worked (maybe)");
            //boatRotation = other.gameObject;
            //boatRotation..rotation.x = (180)
            Quaternion rotation = other.transform.rotation;
            //other.transform.rotation.Set(rotation.x, rotation.y + 180, rotation.z, rotation.w);
            other.transform.Rotate(rotation.x, rotation.y + 180, rotation.z, Space.Self);
        }
    }

}
