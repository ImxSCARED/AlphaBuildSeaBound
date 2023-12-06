using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public string songname;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            AudioManager.instance.PlayNext(songname);
    }
    
}
