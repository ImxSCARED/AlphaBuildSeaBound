using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryTrigger : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<Achievements>().AddScore();
            Destroy(gameObject);
        }
    }
}
