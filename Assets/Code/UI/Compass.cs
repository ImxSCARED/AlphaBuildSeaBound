using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public Transform playerTransform;
    Vector3 dir;

    void Update()
    {
        dir.z = playerTransform.eulerAngles.y - 180;
        transform.localEulerAngles = dir;
    }
}
