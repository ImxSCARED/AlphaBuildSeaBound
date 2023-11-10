using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dock : MonoBehaviour
{
    //For dock tutorial
    public bool dockTutorialShown = false;
    public GameObject dockTutorial;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //For dock tutorial
            dockTutorialShown = true;
            dockTutorial.SetActive(false);

            other.GetComponent<PlayerManager>().Dock();
        }
    }
}
