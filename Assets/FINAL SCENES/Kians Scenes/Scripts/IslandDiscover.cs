using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandDiscover : MonoBehaviour
{

    public string islandName;
    public bool islandDiscoveredStatus = false;

    [SerializeField] private GameObject islandDiscovered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            islandDiscovered.gameObject.SetActive(true);
            this.islandDiscoveredStatus = true;
        }
    }

}
