using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalTutorialUI : MonoBehaviour
{

    [SerializeField] GameObject journalTutorial;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            journalTutorial.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            journalTutorial.SetActive(false);
            Destroy(gameObject);
        }
    }

}
