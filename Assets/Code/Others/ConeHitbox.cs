using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeHitbox : MonoBehaviour
{
    public List<GameObject> EnemiesInRange;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            EnemiesInRange.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemiesInRange.Remove(other.gameObject);
        }
    }
}
