using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public string songname;

    private void OnTriggerEnter(Collision other)
    {
        AudioManager.instance.PlayNext(songname);
    }
    
}
