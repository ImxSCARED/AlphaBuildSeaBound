using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public enum UpgradeType { AMMO, SPEED, WRANGLE, SIZE, RANGE }

    public int AmmoIncreaseAmount;
    [Range(0f, 10f)]
    public float SpeedIncreaseAmount;
    [Range(0f, 10f)]
    public float WrangleIncreaseAmount;
    [Range(0f, 3f)]
    public float CatchingSizeIncreaseAmount;
    [Range(0f, 3f)]
    public float RangeIncreaseAmount;
}
