using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public enum UpgradeType { AMMO, SPEED, TURN, WRANGLE, SIZE, RANGE }

    public int AmmoIncreaseAmount;
    [Range(1f, 10f)]
    public float SpeedIncreaseAmount;
    [Range(1f, 10f)]
    public float TurnIncreaseAmount;
    [Range(1f, 3f)]
    public float WrangleIncreaseAmount;
    [Range(1f, 3f)]
    public float CatchingSizeIncreaseAmount;
    [Range(1f, 3f)]
    public float RangeIncreaseAmount;
}
