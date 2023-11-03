using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade")]
public class Upgrade : ScriptableObject
{
    [Header("Hub Display")]
    public string Name;
    public string Description;
    public int Price;
    

    [Header("Calculations")]
    public int Level;
    public float PriceIncrease;
    public int MaxLevel;

    public UpgradeData.UpgradeType Type;
}
