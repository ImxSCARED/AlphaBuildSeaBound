using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField]
    Transform m_lookPoint;

    [Header("Camera Parameters")]
    [SerializeField]
    Vector2 m_startRotation;
    [SerializeField]
    float m_fishingXRotation;
    [SerializeField]
    float m_minXRotation;
    [SerializeField]
    float m_maxXRotation;
    [SerializeField]
    float m_distanceFromPlayer;
    [SerializeField]
    float m_movementSpeed;
    [SerializeField]
    float m_autoMovementSpeed;
    [SerializeField]
    bool m_invertXControl;
    [SerializeField]
    bool m_invertYControl;

    Vector2 m_XYRotation;

    Vector2 m_xRotationClamp;

    float m_previousXRotation;
    float m_targetXRotation;

    bool m_lerpCamera = false;

    private void Awake()
    {
        m_XYRotation = m_startRotation;

        m_xRotationClamp = new Vector2(m_minXRotation, m_maxXRotation);
    }

    void LateUpdate()
    {
        if (m_lerpCamera)
        {
            m_XYRotation.x = Mathf.Lerp(m_XYRotation.x, m_targetXRotation, Time.deltaTime * m_autoMovementSpeed);

            if (m_XYRotation.x == m_targetXRotation)
            {
                m_lerpCamera = false;
            }
        }
        else
        {
            m_XYRotation.x = Mathf.Clamp(m_XYRotation.x, m_xRotationClamp.x, m_xRotationClamp.y);
        }

        Quaternion cameraRotation = Quaternion.Euler(m_XYRotation.x, m_XYRotation.y, 0);

        transform.rotation = cameraRotation;
        transform.position = m_lookPoint.position + cameraRotation * (m_distanceFromPlayer * -Vector3.forward);
    }

    /// <summary>
    /// Rotates camera in an orbit around the ship
    /// </summary>
    /// <param name="x">The direction to move the camera on the x axis, from -1 to 1</param>
    /// <param name="y">The direction to move the camera on the y axis, from -1 to 1</param>
    public void RotateCamera(float x, float y)
    {
        if (!m_lerpCamera)
        {
            float deltaX = (m_invertXControl ? -x : x) * m_movementSpeed * Time.deltaTime;
            float deltaY = (m_invertYControl ? -y : y) * m_movementSpeed * Time.deltaTime;

            m_XYRotation.x += deltaX;
            m_XYRotation.y += deltaY;
        }
    }

    /// <summary>
    /// Rotates camera in an orbit around the ship
    /// </summary>
    /// <param name="rotation">Strength of camera movement, from (-1,-1) to (1,1). X is X axis rotation, Y is Y axis rotation</param>
    public void RotateCamera(Vector2 rotation)
    {
        RotateCamera(rotation.x, rotation.y);
    }

    /// <summary>
    /// Rotates camera in an orbit around the ship
    /// </summary>
    /// <param name="rotation">Quaternion roation to move the camera by (only considers x and y euler angles)</param>
    public void RotateCamera(Quaternion rotation)
    {
        Vector3 movementEulers = rotation.eulerAngles;

        RotateCamera(movementEulers.x, movementEulers.y);
    }

    /// <summary>
    /// Adds a flat rotation to the camera's rotation
    /// </summary>
    /// <param name="x">Amount of x-axis rotation to add</param>
    /// <param name="y">Amount of y-axis rotation to add</param>
    public void AddRotation(float x, float y)
    {
        if (!m_lerpCamera)
        {
            m_XYRotation.x += x;
            m_XYRotation.y += y;
        }
    }

    /// <summary>
    /// Adds a flat rotation to the camera's rotation
    /// </summary>
    /// <param name="deltaRotation">Amount of x-axis and y-axis rotation to add</param>
    public void AddRotation(Vector2 deltaRotation)
    {
        AddRotation(deltaRotation.x, deltaRotation.y);
    }

    /// <summary>
    /// Adds a flat rotation to the camera's rotation
    /// </summary>
    /// <param name="deltaRotation">Quaternion to add (only considers x and y euler angles)</param>
    public void AddRotation(Quaternion deltaRotation)
    {
        // TODO: Movement makes camera jittery - need to find a way to interpolate between old and new rotation/position like unity does
        Vector3 rotationEulers = deltaRotation.eulerAngles;

        AddRotation(rotationEulers.x, rotationEulers.y);
    }

    /// <summary>
    /// Sets the camera's rotation around the ship
    /// </summary>
    /// <param name="x">Rotation around the x axis</param>
    /// <param name="y">Rotation around the y axis</param>
    public void SetCameraRotation(float x, float y)
    {
        if (!m_lerpCamera)
        {
            m_XYRotation.x = x;
            m_XYRotation.y = y;
        }
    }

    /// <summary>
    /// Sets the camera's rotation around the ship
    /// </summary>
    /// <param name="rotation">Rotation around the x and y axes</param>
    public void SetCameraRotation(Vector2 rotation)
    {
        SetCameraRotation(rotation.x, rotation.y);
    }

    /// <summary>
    /// Sets the camera's rotation around the ship
    /// </summary>
    /// <param name="rotation">Rotation quaternion (only considers x and y euler angles)</param>
    public void SetCameraRotation(Quaternion rotation)
    {
        Vector3 rotationEulers = rotation.eulerAngles;

        SetCameraRotation(rotationEulers.x, rotationEulers.y);
    }

    public void StartFishingMode()
    {
        m_previousXRotation = m_XYRotation.x;
        m_targetXRotation = m_fishingXRotation;

        m_xRotationClamp = new Vector2(m_fishingXRotation, m_fishingXRotation);

        m_lerpCamera = true;
    }

    public void EndFishingMode()
    {
        m_targetXRotation = m_previousXRotation;

        m_xRotationClamp = new Vector2(m_minXRotation, m_maxXRotation);

        m_lerpCamera = true;
    }
}
