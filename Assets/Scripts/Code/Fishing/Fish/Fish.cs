using static FishProperties;
using UnityEngine;
using System.Collections.Generic;

public class Fish : MonoBehaviour
{
    [SerializeField] private FishProperties properties;
    public FishTier tier;

    private float timeBeforeAction = 0;
    private float actionDuration = 0;
    private Transform fishingHitboxNode;

    /// <summary>
    /// Based on tier, moves to a random node after a time limit to make fishing harder
    /// </summary>
    public void FishDash(Transform[] hitboxNodes, bool isFishBeingReeled)
    {
        timeBeforeAction += Time.deltaTime;
        switch (tier)
        {
            case FishTier.SMALL:
                if(timeBeforeAction > properties.smallFish.timeBeforeDash)
                {
                    if (!fishingHitboxNode)
                    {
                        fishingHitboxNode = hitboxNodes[Random.Range(0, (int)hitboxNodes.Length - 1)];
                    }

                    actionDuration += Time.deltaTime;
                    if(actionDuration < properties.smallFish.dashDuration)
                    {
                        if (isFishBeingReeled)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, fishingHitboxNode.position, properties.smallFish.dashWhileWrangledSpeed * Time.deltaTime);
                        }
                        else
                        {
                            transform.position = Vector3.MoveTowards(transform.position, fishingHitboxNode.position, properties.smallFish.dashSpeed * Time.deltaTime);
                        }
                        
                    }
                    else
                    {
                        actionDuration = 0;
                        timeBeforeAction = 0;
                        fishingHitboxNode = null;
                    }
                }
                break;

            case FishTier.MEDIUM:
                if (timeBeforeAction > properties.mediumFish.timeBeforeDash)
                {
                    if (!fishingHitboxNode)
                    {
                        fishingHitboxNode = hitboxNodes[Random.Range(0, (int)hitboxNodes.Length - 1)];
                    }

                    actionDuration += Time.deltaTime;
                    if (actionDuration < properties.mediumFish.dashDuration)
                    {
                        if (isFishBeingReeled)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, fishingHitboxNode.position, properties.mediumFish.dashWhileWrangledSpeed * Time.deltaTime);
                        }
                        else
                        {
                            transform.position = Vector3.MoveTowards(transform.position, fishingHitboxNode.position, properties.mediumFish.dashSpeed * Time.deltaTime);
                        }

                    }
                    else
                    {
                        actionDuration = 0;
                        timeBeforeAction = 0;
                        fishingHitboxNode = null;
                    }
                }
                break;

            case FishTier.LARGE:
                if (timeBeforeAction > properties.largeFish.timeBeforeDash)
                {
                    if (!fishingHitboxNode)
                    {
                        fishingHitboxNode = hitboxNodes[Random.Range(0, (int)hitboxNodes.Length - 1)];
                    }

                    actionDuration += Time.deltaTime;
                    if (actionDuration < properties.largeFish.dashDuration)
                    {
                        if (isFishBeingReeled)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, fishingHitboxNode.position, properties.largeFish.dashWhileWrangledSpeed * Time.deltaTime);
                        }
                        else
                        {
                            transform.position = Vector3.MoveTowards(transform.position, fishingHitboxNode.position, properties.largeFish.dashSpeed * Time.deltaTime);
                        }

                    }
                    else
                    {
                        actionDuration = 0;
                        timeBeforeAction = 0;
                        fishingHitboxNode = null;
                    }
                }
                break;
        }
    }
}
