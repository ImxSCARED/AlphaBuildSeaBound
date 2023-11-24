using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearIslandPopup : MonoBehaviour
{
    public IslandInfo m_IslandInfo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerManager>().IslandNamePopup(m_IslandInfo.islandName, m_IslandInfo.islandDesc);
        }
    }
}
