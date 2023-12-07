//Author: Jamie Wright
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Island Info")]
public class IslandInfo : ScriptableObject
{
    public string islandName;
    [TextArea(15, 20)]
    public string islandDesc;
    public bool discovered;
}
