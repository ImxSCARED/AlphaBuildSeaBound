using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Fish Properties")]
public class FishProperties : ScriptableObject
{
    [System.Serializable]
    public struct FishData
    {
        public string name;
        public Image fishImage;
        public int value;
        public FishTier tier;
    }
    [System.Serializable]
    public struct FishCalculations
    {
        public float dashSpeed;
        public float dashWhileWrangledSpeed;
        [Range(0f, 5f)]
        public float timeBeforeDash;
        [Range(0f, 5f)]
        public float dashDuration;
        public FishData[] fishies;
    }
    public enum FishTier { SMALL, MEDIUM, LARGE }
    public FishCalculations smallFish;
    public FishCalculations mediumFish;
    //Can make this an array later, and get a random one to maybe make discernable fish patterns
    public FishCalculations largeFish;

}
