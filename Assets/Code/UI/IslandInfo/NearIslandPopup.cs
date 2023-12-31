using UnityEngine;
using UnityEngine.UI;

public class NearIslandPopup : MonoBehaviour
{
    public IslandInfo m_IslandInfo;
    public Button assignedMapButton;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(other.gameObject.name);
            other.GetComponent<PlayerManager>().IslandNamePopup(m_IslandInfo.islandName);
            if (!m_IslandInfo.discovered)
            {
                m_IslandInfo.discovered = true;
                assignedMapButton.interactable = true;
            }
            
        }
    }
}
