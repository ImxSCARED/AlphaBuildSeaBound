using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField]
    GameObject m_cameraTarget;
    [SerializeField]
    Vector3 m_offset;

    void Update()
    {
        transform.position = m_cameraTarget.transform.position + m_offset;
    }
}
