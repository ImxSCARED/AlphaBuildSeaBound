using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    private void Awake()
    {
        Invoke("Delay", 6);
        
    }

    private void Delay()
    {
        SceneManager.LoadScene("Myles_Beta_Scene");
    }
}


