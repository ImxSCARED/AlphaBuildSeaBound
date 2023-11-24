using UnityEngine;

public class FogReverse : MonoBehaviour
{

    [SerializeField] private CameraController PlayerCamera;

    // Rotating the player and camera when the player hits a boundary.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Quaternion rotation = other.transform.rotation;

            other.transform.Rotate(rotation.x, rotation.y + 180, rotation.z, Space.Self);

            PlayerCamera.AddRotation(0, 180);
        }
    }

}
