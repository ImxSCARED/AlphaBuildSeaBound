using static FishProperties;
using UnityEngine;
using System.Collections.Generic;

public class Fish : MonoBehaviour
{
    [SerializeField] private FishProperties properties;
    public FishTier tier;

    public FishData data;
    private float timeBeforeAction = 0;
    private float actionDuration = 0;
    private Vector3 fishingHitboxNode;

    public bool dontDestory; // If its being fished, game shouldnt remove it when respawning fish
    /// <summary>
    /// Based on tier, moves to a random node after a time limit to make fishing harder
    /// </summary>
    private void Awake()
    {
        data = properties.GetFishData(tier);
    }
    public void FishDash(Vector3[] hitboxNodes, bool isFishBeingReeled)
    {
        timeBeforeAction += Time.deltaTime;
        switch (tier)
        {
            case FishTier.SMALL:
                if(timeBeforeAction > properties.smallFish.timeBeforeDash)
                {
                    if (fishingHitboxNode == Vector3.zero)
                    {
                        fishingHitboxNode = hitboxNodes[Random.Range(0, hitboxNodes.Length)];
                    }

                    actionDuration += Time.deltaTime;
                    if(actionDuration < properties.smallFish.dashDuration)
                    {
                        if (isFishBeingReeled)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, fishingHitboxNode, properties.smallFish.dashWhileWrangledSpeed * Time.deltaTime);
                        }
                        else
                        {
                            transform.position = Vector3.MoveTowards(transform.position, fishingHitboxNode, properties.smallFish.dashSpeed * Time.deltaTime);
                        }
                        
                    }
                    else
                    {
                        actionDuration = 0;
                        timeBeforeAction = 0;
                        fishingHitboxNode = Vector3.zero;
                    }
                }
                break;

            case FishTier.MEDIUM:
                if (timeBeforeAction > properties.mediumFish.timeBeforeDash)
                {
                    if (fishingHitboxNode == Vector3.zero)
                    {
                        fishingHitboxNode = hitboxNodes[Random.Range(0, hitboxNodes.Length)];
                    }

                    actionDuration += Time.deltaTime;
                    if (actionDuration < properties.mediumFish.dashDuration)
                    {
                        if (isFishBeingReeled)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, fishingHitboxNode, properties.mediumFish.dashWhileWrangledSpeed * Time.deltaTime);
                        }
                        else
                        {
                            transform.position = Vector3.MoveTowards(transform.position, fishingHitboxNode, properties.mediumFish.dashSpeed * Time.deltaTime);
                        }

                    }
                    else
                    {
                        actionDuration = 0;
                        timeBeforeAction = 0;
                        fishingHitboxNode = Vector3.zero;
                    }
                }
                break;

            case FishTier.LARGE:
                if (timeBeforeAction > properties.largeFish.timeBeforeDash)
                {
                    if (fishingHitboxNode == Vector3.zero)
                    {
                        fishingHitboxNode = hitboxNodes[Random.Range(0, hitboxNodes.Length)];
                    }

                    actionDuration += Time.deltaTime;
                    if (actionDuration < properties.largeFish.dashDuration)
                    {
                        if (isFishBeingReeled)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, fishingHitboxNode, properties.largeFish.dashWhileWrangledSpeed * Time.deltaTime);
                        }
                        else
                        {
                            transform.position = Vector3.MoveTowards(transform.position, fishingHitboxNode, properties.largeFish.dashSpeed * Time.deltaTime);
                        }

                    }
                    else
                    {
                        actionDuration = 0;
                        timeBeforeAction = 0;
                        fishingHitboxNode = Vector3.zero;
                    }
                }
                break;
        }
    }
}
