using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class FishingHitbox : MonoBehaviour
{
    public PlayerManager playerManager;
    public LineRenderer lineRenderer;

    public GameObject currentFish = null;
    public Transform MinigameMover;

    private bool isCircleInZone;
    private Vector3 lastPosition;
    [HideInInspector] public Vector3[] leftPoints;
    [HideInInspector] public Vector3[] rightPoints;

    public float radius;
    public float widthRatio; // Change to account for rotation
    public float heightRatio; // Change to account for rotation
    public int boundsLOD;
    private float ovalExtant;
    private float rotationRad;

    private GameObject[] fishInScene;
    private GameObject closestFish;

    private void Start()
    {
        //radius = fishingHitboxCollider.bounds.extents.x;

        //widthRatio = 1;
        //heightRatio = fishingHitboxCollider.bounds.extents.z/radius;

        lineRenderer.positionCount = boundsLOD;
        RedrawOval();

        ResetDashPoints();

        fishInScene = GameObject.FindGameObjectsWithTag("Fish");
    }
    private void Update()
    {
        // Set rotationRad for this frame
        rotationRad = Mathf.Deg2Rad * transform.rotation.eulerAngles.y;

        // Check if closest fish is in the oval
        FindClosestFish();

        if (IsPointInOval(closestFish.transform.position - transform.position))
        {
            currentFish = closestFish;
            currentFish.GetComponent<Fish>().dontDestory = true;
        }
        else if (currentFish)
        {
            currentFish.GetComponent<Fish>().dontDestory = false;
            currentFish = null;
        }

        // If minigame mover leaves oval, move it back in - Emma
        Vector3 circlePos = MinigameMover.position - transform.position;
        
        if (!IsPointInOval(circlePos))
        {
            MinigameMover.position = lastPosition;
        }

        lastPosition = MinigameMover.position;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Fish")
        {
            currentFish = collision.gameObject;
            currentFish.GetComponent<Fish>().dontDestory = true;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (currentFish == collision.gameObject)
        {
            currentFish.GetComponent<Fish>().dontDestory = false;
            currentFish = null;
        }
    }

    private void FindClosestFish()
    {
        float closestFishDist = 0;
        closestFish = null;

        // When we start spawning fish, use playerManager.fishOnMap instead of fishInScene
        foreach (GameObject fish in fishInScene)
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
    public bool IsPointInOval(Vector3 point)
    {
        float distanceSquared = (Mathf.Pow((point.x * Mathf.Cos(rotationRad)) - (point.z * Mathf.Sin(rotationRad)), 2) / Mathf.Pow(widthRatio, 2)) +
                                (Mathf.Pow((point.x * Mathf.Sin(rotationRad)) + (point.z * Mathf.Cos(rotationRad)), 2) / Mathf.Pow(heightRatio, 2));

        if (distanceSquared <= Mathf.Pow(radius, 2))
        {
            return true;
        }

        return false;
    }

    public bool MagnitudeIsInOval(float magnitude)
    {
        return magnitude <= ovalExtant;
    }

    public void RedrawOval()
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
        leftPoints = new Vector3[Mathf.CeilToInt(lineRenderer.positionCount / 2f)];
        rightPoints = new Vector3[Mathf.CeilToInt(lineRenderer.positionCount / 2f)];

        int leftPointsCounter = 0;
        int rightPointsCounter = 0;

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            // Points start from the rightmost position, so we have to do some weird checks to make this work

            if (i > (lineRenderer.positionCount * 0.25f) - 1 && i < (lineRenderer.positionCount * 0.75f) - 1)
            {
                // If the position is in the second or third quarter
                leftPoints[leftPointsCounter] = lineRenderer.GetPosition(i) + transform.position;
                leftPointsCounter++;
            }
            else if (i > (lineRenderer.positionCount * 0.75f) - 1 || i < (lineRenderer.positionCount * 0.25f) - 1)
            {
                // If the position is in the first or fourth quarter
                rightPoints[rightPointsCounter] = lineRenderer.GetPosition(i) + transform.position;
                rightPointsCounter++;
            }
        }
    }
}
