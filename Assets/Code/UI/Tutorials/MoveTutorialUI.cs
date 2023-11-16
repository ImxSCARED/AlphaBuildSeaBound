using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTutorialUI : MonoBehaviour
{

    [SerializeField] private TutorialManager moveTutorial;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            moveTutorial.StartMoveTutorial();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            moveTutorial.StopMoveTutorial();
            Destroy(gameObject);
        }
    }

}
