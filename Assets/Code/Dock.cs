using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dock : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerManager>().isAtDock = true;
            AudioManager.instance.PlaySound("AmmoUp");
            AudioManager.instance.PlayNextClip("Shop");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerManager>().isAtDock = false;
            AudioManager.instance.PlayNextClip("Still");
        }
    }

}
    