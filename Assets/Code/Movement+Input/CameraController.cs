using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform m_lookPoint;
    [SerializeField]
    Camera m_camera;

    [SerializeField]
    float m_speed;

    /// <summary>
    /// Moves camera in an orbit around the ship
    /// </summary>
    /// <param name="around">The direction to move the camera around the ship, from -1 to 1</param>
    /// <param name="up">The direction to move the camera up and down, from -1 to 1</param>
    public void MoveCamera(float around, float up)
    {
        around *= m_speed * Time.deltaTime;
        up *= m_speed * Time.deltaTime;

        transform.position += around * transform.right + up * transform.up;
        transform.LookAt(m_lookPoint.position);
    }

    /// <summary>
    /// Moves camera in an orbit around the ship
    /// </summary>
    /// <param name="movement">Strength of camera movement, from (-1,-1) to (1,1). Y is up, X is around.</param>
    public void MoveCamera(Vector2 movement)
    {
        movement.x *= m_speed * Time.deltaTime;
        movement.y *= m_speed * Time.deltaTime;

        transform.position += movement.x * transform.right + movement.y * transform.up;
        transform.LookAt(m_lookPoint.position);
    }
}
