using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD

public class Quest : ScriptableObject
{
    public string Name;
    public PlayerManager.ZoneLevel Zone;

=======
using UnityEngine.Experimental.GlobalIllumination;

[CreateAssetMenu(menuName = "Quest")]
public class Quest : ScriptableObject
{
    public string Name;
    public int Value;
    public PlayerManager.ZoneLevel Zone;

    public Upgrade connectedUpgrade;

    public FishProperties.FishData connectedFish;
    public bool completed;
>>>>>>> Jamies_Branch
}
