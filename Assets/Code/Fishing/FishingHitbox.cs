using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingHitbox : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public GameObject currentFish = null;
    public Transform MinigameMover;

    public Collider fishingHitboxCollider;

    private bool isCircleInZone;
    private Vector3 lastPosition;
    public Transform[] leftPoints;
    public Transform[] rightPoints;

    public float radius;
    public float mainWidthRatio; // Change to account for rotation
    public float mainHeightRatio; // Change to account for rotation
    private float ovalExtant;

    private void Start()
    {
        radius = fishingHitboxCollider.bounds.extents.x;

        mainWidthRatio = 1;
        mainHeightRatio = fishingHitboxCollider.bounds.extents.z/radius;
    }
    private void Update()
    {
        // Calculate oval extants - Emma
        Vector3 circlePos = MinigameMover.position - transform.position;
        
        float widthRatio = mainWidthRatio; // Change to account for rotation
        float heightRatio = mainHeightRatio; // Change to account for rotation

        float rotationRad = Mathf.Deg2Rad * transform.rotation.eulerAngles.y - 90;

        float distanceFromMid = (Mathf.Pow((circlePos.x * Mathf.Cos(rotationRad)) - (circlePos.z * Mathf.Sin(rotationRad)), 2) / Mathf.Pow(widthRatio, 2)) +
                                (Mathf.Pow((circlePos.x * Mathf.Sin(rotationRad)) + (circlePos.z * Mathf.Cos(rotationRad)), 2) / Mathf.Pow(heightRatio, 2));

        // If minigame mover leaves oval, move it back in - Emma
        if (distanceFromMid > Mathf.Pow(radius, 2))
        {
            MinigameMover.position = lastPosition;
        }

        lastPosition = MinigameMover.position;

        // Draw oval with line renderer
        
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

    public bool PointIsInOval(Vector3 point)
    {
        return Vector3.Distance(point, transform.position) <= ovalExtant;
    }

    public bool MagnitudeIsInOval(float magnitude)
    {
        return magnitude <= ovalExtant;
    }
}
