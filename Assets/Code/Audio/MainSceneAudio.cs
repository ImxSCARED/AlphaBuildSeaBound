using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneAudio : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        FindObjectOfType<AudioManager>().PlayNext("Track_Shallows");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
