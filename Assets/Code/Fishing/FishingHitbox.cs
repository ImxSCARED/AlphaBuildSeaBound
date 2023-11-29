using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class FishingHitbox : MonoBehaviour
{
    public PlayerManager playerManager;
    public LineRenderer fishingLineRenderer;
    public LineRenderer antiFishingLineRenderer;

    public GameObject currentFish = null;
    public Transform MinigameMover;

    private Vector3 lastPosition;
    [HideInInspector] public Vector3[] leftPoints;
    [HideInInspector] public Vector3[] rightPoints;

    [Header("Fishing Oval")]
    public float fishingRadius;
    public float fishingWidthRatio;
    public float fishingHeightRatio;
    public int fishingBoundsLOD;

    [Header("Anti-Fishing oval")]
    public float antiFishingRadius;
    public float antiFishingWidthRatio;
    public float antiFishingHeightRatio;
    public int antiFishingBoundsLOD;

    public bool inAntiFishingOval;

    private float rotationRad;

    private GameObject closestFish;

    //Tutorial
    [SerializeField] private TutorialManager fishingTutorial;
    [SerializeField] private Fishing fishingState;

    private void Start()
    {
        fishingLineRenderer.positionCount = fishingBoundsLOD;
        antiFishingLineRenderer.positionCount = antiFishingBoundsLOD;

        RedrawOval(fishingLineRenderer, fishingRadius, fishingWidthRatio, fishingHeightRatio);
        RedrawOval(antiFishingLineRenderer, antiFishingRadius, antiFishingWidthRatio, antiFishingHeightRatio);

        ResetDashPoints();
    }
    private void Update()
    {
        // Set rotationRad for this frame
        rotationRad = Mathf.Deg2Rad * transform.rotation.eulerAngles.y;

        // Find the closest fish...
        FindClosestFish();

        //...and check that it's in the fishing oval, and not in the anti-fishing oval
        if (IsPointInOval(closestFish.transform.position - transform.position, fishingRadius, fishingWidthRatio, fishingHeightRatio))
        {
            currentFish = closestFish;
            
            if (!fishingState.currentlyFishing)
            {
                fishingTutorial.StartFishTutorial();
            }

            currentFish.GetComponent<Fish>().dontDestory = true;
        }
        else if (currentFish)
        {
            currentFish.GetComponent<Fish>().dontDestory = false;
            currentFish = null;
            fishingTutorial.StopFishTutorial();
        }
        // Check that the fish is in the anti-fishing oval
        inAntiFishingOval = IsPointInOval(closestFish.transform.position - transform.position, antiFishingRadius, antiFishingWidthRatio, antiFishingHeightRatio);

        // If minigame mover leaves oval, move it back in - Emma
        Vector3 circlePos = MinigameMover.position - transform.position;

        // Check that minigame mover is not in the fishing oval, or is in the anti-fishing oval
        if (!IsPointInOval(circlePos, fishingRadius, fishingWidthRatio, fishingHeightRatio))
        {
            MinigameMover.position = lastPosition;
        }

        lastPosition = MinigameMover.position;
    }

    private void FindClosestFish()
    {
        float closestFishDist = 0;
        closestFish = null;

        foreach (GameObject fish in playerManager.fishOnMap)
        {
            float distanceFromShip = (fish.transform.position - transform.position).magnitude;

            if (closestFish)
            {
                if (distanceFromShip < closestFishDist)
                {
                    closestFishDist = distanceFromShip;
                    closestFish = fish;
                }
            }
            else
            {
                closestFishDist = distanceFromShip;
                closestFish = fish;
            }
        }
    }

    /// <summary>
    /// Checks if a point is in the ellipse bounding box
    /// </summary>
    /// <param name="point">World pos of point to check</param>
    /// <returns>Whether or not the point is in the oval</returns>
    public bool IsPointInOval(Vector3 point, float radius, float widthRatio, float heightRatio)
    {
        float distanceSquared = (Mathf.Pow((point.x * Mathf.Cos(rotationRad)) - (point.z * Mathf.Sin(rotationRad)), 2) / Mathf.Pow(widthRatio, 2)) +
                                (Mathf.Pow((point.x * Mathf.Sin(rotationRad)) + (point.z * Mathf.Cos(rotationRad)), 2) / Mathf.Pow(heightRatio, 2));

        if (distanceSquared <= Mathf.Pow(radius, 2))
        {
            return true;
        }

        return false;
    }

    public void RedrawOval(LineRenderer lineRenderer, float radius, float widthRatio, float heightRatio)
    {
        float widthRad = radius * widthRatio;
        float heightRad = radius * heightRatio;

        float twoPi = Mathf.PI * 2;

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            float theta = twoPi * ((i + 1) / (float)lineRenderer.positionCount);
            float newX = widthRad * Mathf.Cos(theta);
            float newZ = heightRad * Mathf.Sin(theta);

            Vector3 newPos = new(newX, 0, newZ);

            lineRenderer.SetPosition(i, newPos);
        }
    }

    public void ResetDashPoints()
    {
        // Set left and right fish dash points
        leftPoints = new Vector3[Mathf.CeilToInt(fishingLineRenderer.positionCount / 2f)];
        rightPoints = new Vector3[Mathf.CeilToInt(fishingLineRenderer.positionCount / 2f)];

        int leftPointsCounter = 0;
        int rightPointsCounter = 0;

        for (int i = 0; i < fishingLineRenderer.positionCount; i++)
        {
            // Points start from the rightmost position, so we have to do some weird checks to make this work

            if (i > (fishingLineRenderer.positionCount * 0.25f) - 1 && i < (fishingLineRenderer.positionCount * 0.75f) - 1)
            {
                // If the position is in the second or third quarter
                leftPoints[leftPointsCounter] = fishingLineRenderer.GetPosition(i) + transform.position;
                leftPointsCounter++;
            }
            else if (i > (fishingLineRenderer.positionCount * 0.75f) - 1 || i < (fishingLineRenderer.positionCount * 0.25f) - 1)
            {
                // If the position is in the first or fourth quarter
                rightPoints[rightPointsCounter] = fishingLineRenderer.GetPosition(i) + transform.position;
                rightPointsCounter++;
            }
        }
    }
}
