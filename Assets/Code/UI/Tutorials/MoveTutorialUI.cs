using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTutorialUI : MonoBehaviour
{

    [SerializeField] GameObject moveTutorial;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            moveTutorial.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            moveTutorial.SetActive(false);
            Destroy(gameObject);
        }
    }

}
