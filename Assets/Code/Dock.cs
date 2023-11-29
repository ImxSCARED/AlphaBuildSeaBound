using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dock : MonoBehaviour
{
<<<<<<< HEAD

=======
>>>>>>> a16cac6f05253236945936fcca969b39581947c1
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
<<<<<<< HEAD


=======
>>>>>>> a16cac6f05253236945936fcca969b39581947c1
            other.GetComponent<PlayerManager>().isAtDock = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerManager>().isAtDock = false;
        }
    }

}
    