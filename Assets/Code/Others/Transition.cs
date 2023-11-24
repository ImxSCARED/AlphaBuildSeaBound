using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{

    
    void Awake()
    {
        Invoke("FogOut", 1);
    }

    void FogOut()
    {
        GetComponent<ParticleSystem>().Stop(false);
    }


}
