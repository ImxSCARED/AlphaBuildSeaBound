using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fishing : MonoBehaviour
{
    //Outside objects
    [SerializeField] private FishingHitbox fishingSpot;
    public CaptureCircle minigameBackground;
    public GameObject minigameMover;
    public Bounds bounds;

    //Fishing
    private Fish currentFish;
    public bool currentlyFishing;
    public int currentHarpoons = 6;

    //Values changed by UpgradeManager
    public int ammoUpgrade = 0;
    public float wrangleUpgrade = 0;
    public float rangeUpgrade = 1f;

    //Testing and balancing
    [Header("Exposed Variables")]
    public int maxHarpoons = 6;
    [Range(1f, 100f)]
    public float fishWrangleSpeed = 3f;
    [Range(0f, 10f)]
    public float fishMovingAwaySpeed = 1.5f;
    public Vector2 fishingRange = new Vector2(30, 20);
    public void FishMinigame()
    {
        if (!currentlyFishing)
        {
            if (fishingSpot.currentFish)
            {
                if(currentHarpoons > 0)
                {
                    GetComponent<InputManager>().ChangeActionMap("Fishing");
                    currentlyFishing = true;
                    minigameMover.SetActive(true);
                    minigameMover.transform.position = new Vector3(fishingSpot.currentFish.transform.position.x, minigameMover.transform.position.y, fishingSpot.currentFish.transform.position.z);
                    currentFish = fishingSpot.currentFish.GetComponent<Fish>();
                    currentHarpoons--;
                }
                else
                {
                    //Put in warning to player that they have no ammo
                }
            }
        }
    }

    public void EndMinigame(bool fishCaught)
    {
        GetComponent<InputManager>().ChangeActionMap("Sailing");
        currentlyFishing = false;
        minigameMover.SetActive(false);

        if (fishCaught)
        {
            GameObject fish = fishingSpot.currentFish;
            GetComponent<PlayerManager>().AddFish(fish.GetComponent<Fish>().data);

            fishingSpot.currentFish = null;
            fish.SetActive(false);

        }
    }

    private void Update()
    {
        bounds = fishingSpot.GetComponent<MeshCollider>().bounds;
        if (currentlyFishing)
        {
            if (fishingSpot.currentFish == null)
            {
                EndMinigame(false);
                return;
            }

            //Checks whether the fish is on the left or rightside of the boat, then give the fish the relative nodes to dash to
            Vector3 perp = Vector3.Cross(transform.forward, fishingSpot.currentFish.transform.position - transform.position);
            float dir = Vector3.Dot(perp, transform.up);
            if (dir > 0f)
            {
                currentFish.FishDash(fishingSpot.rightPoints, minigameBackground.fishInCircle);
            }
            else
            {
                currentFish.FishDash(fishingSpot.leftPoints, minigameBackground.fishInCircle);
            }

            //If fish is in biscut, wrangle it towards ship
            if (minigameBackground.fishInCircle)
            {
                Vector3 moveAwayDistance = Vector3.MoveTowards(currentFish.transform.position, transform.position, (fishWrangleSpeed + wrangleUpgrade) * Time.deltaTime);
                currentFish.transform.position = new Vector3(moveAwayDistance.x, currentFish.transform.position.y, moveAwayDistance.z);
            }
            else
            {
                Vector3 moveAwayDistance = Vector3.MoveTowards(currentFish.transform.position, transform.position, -fishMovingAwaySpeed * Time.deltaTime);
                currentFish.transform.position = new Vector3(moveAwayDistance.x, currentFish.transform.position.y, moveAwayDistance.z);
            }
        }
    }

    public void ReplenishHarpoons()
    {
        currentHarpoons = maxHarpoons + ammoUpgrade;
    }

    public void ChangeFishingRangeSize()
    {
        fishingSpot.transform.localScale = new Vector3(fishingRange.x * rangeUpgrade, 0.01f, fishingRange.y * rangeUpgrade);
    }
    public void MoveMM(Vector2 input)
    {
        minigameMover.transform.position += minigameMover.transform.forward * (input.y * Time.deltaTime) * 5;
        minigameMover.transform.position += minigameMover.transform.right * (input.x * Time.deltaTime) * 5;
    }
}