using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCaller : MonoBehaviour
{
    
    //copy and use below code to call new sounds for buttons/event triggers

   public void CallSound(string name)
    {
        AudioManager.instance.PlaySound(name);
    }







}
