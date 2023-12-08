
using UnityEngine;

[CreateAssetMenu(menuName = "Fish Properties")]
public class FishProperties : ScriptableObject
{
    [System.Serializable]
    public struct FishData
    {
        public string name;
        public Sprite fishImage;
        public int value;
        public bool isQuestFish;
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

    public FishData GetFishData(FishTier tier)
    {
        switch (tier)
        {
            case FishTier.SMALL:
                return smallFish.fishies[Random.Range(0, smallFish.fishies.Length)];
            case FishTier.MEDIUM:
                return mediumFish.fishies[Random.Range(0, mediumFish.fishies.Length)];
            case FishTier.LARGE:
                return largeFish.fishies[Random.Range(0, largeFish.fishies.Length)];
        }
        Debug.Log("GetFishData switch was avoided");
        return smallFish.fishies[0];
    }
}
