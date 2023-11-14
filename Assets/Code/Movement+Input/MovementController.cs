using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class MovementController : MonoBehaviour
{
    // --EDITOR VARIABLES--
    [Header("Object References")]
    [SerializeField]
    Rigidbody m_rigidbody;
    [SerializeField]
    Transform m_rotationAxis;
    [SerializeField]
    BoxCollider m_collider;

    [Header("Player Control")]

    // Acceleration
    [SerializeField]
    [Tooltip("Maximum force that the engine can impart on the player")]
    [Min(0f)]
    float m_engineForce;
    [SerializeField]
    [Tooltip("Speed at which the engine's power increases per second in percentages (e.g. 200% per second)")]
    [Min(0f)]
    float m_accelerationSpeed;

    // Turning
    [SerializeField]
    [Tooltip("Rate at which ship turning accelerates per second in degrees. Increase to account for turn friction")]
    [Min(0f)]
    float m_turnSpeed;

    [Header("Forces")]

    [SerializeField]
    [Tooltip("Maximum velocity the ship can reach. Cannot be less than m_engineForce.")]
    [Min(0f)]
    float m_maxVelocity;

    // Turning
    [SerializeField]
    [Tooltip("Percentage of velocity maintained in the forward direction when turning")]
    [Range(0f, 100f)]
    float m_turningVelocityRetained;

    // Friction
    [SerializeField]
    [Tooltip("Rate at which the boat slows in the horizontal directions")]
    [Min(0f)]
    float m_sideFriction;

    // Collision
    [SerializeField]
    [Tooltip("Percentage of momentum maintained in a collision")]
    [Range(0f, 100f)]
    float m_collisionMomentumRetained;

    // --CODE VARIABLES--

    // Velocity
    Vector3 m_velocity;

    // Engine
    float m_enginePower; // Percentage value, from 0% - 100%

    void Start()
    {
        if (m_maxVelocity < m_engineForce)
        {
            m_maxVelocity = m_engineForce;
        }
    }

    void FixedUpdate()
    {
        ApplyEngine();

        // Clamp velocity
        m_velocity = ClampMagnitude3(m_velocity, -m_maxVelocity, m_maxVelocity);
        m_velocity.y = 0;

        // Apply friction
        ApplyFriction();

        MoveWithCollision(m_velocity * Time.deltaTime);
    }

    /// <summary>
    /// Adds a vector3 force to velocity, mostly used internally
    /// </summary>
    /// <param name="force">Force to apply</param>
    public void AddForce(Vector3 force)
    {
        m_velocity += force * Time.deltaTime;
    }

    /// <summary>
    /// Causes the engine to accelerate by magnitude * m_accelerationSpeed
    /// </summary>
    /// <param name="magnitude">Amount from -1 to 1 that the engine should accelerate</param>
    public void Accelerate(float magnitude)
    {
        magnitude = Mathf.Clamp(magnitude, -1f, 1f);

        float deltaEngine = magnitude * m_accelerationSpeed * Time.deltaTime;
        m_enginePower += deltaEngine;

        m_enginePower = Mathf.Clamp(m_enginePower, 0f, 100f);
    }

    /// <summary>
    /// Rotates the boat by magnitude * m_turnSpeed
    /// TODO: make this rotate around custom axis
    /// TODO: ensure the turning works properly with speed
    /// TODO: Add collision to rotation (RotateWithCollision?)
    /// </summary>
    /// <param name="magnitude">Amount from -1 to 1 that the boat should rotate</param>
    public void Turn(float magnitude)
    {
        magnitude = Mathf.Clamp(magnitude, -1f, 1f);

        float deltaTurn = magnitude * m_turnSpeed * Mathf.Lerp(0, 1, m_enginePower/100);

        m_rigidbody.MoveRotation(m_rigidbody.rotation * Quaternion.Euler(0f, deltaTurn * Mathf.Deg2Rad, 0));

        float decimalPercentTVR = m_turningVelocityRetained / 100;
        m_velocity = (transform.forward * m_velocity.magnitude * decimalPercentTVR) + (m_velocity * (1 - decimalPercentTVR));
    }

    /// <summary>
    /// Moves the boat by some movement, with collision behaviour
    /// </summary>
    /// <param name="movement">Amount to move player</param>
    public void MoveWithCollision(Vector3 movement)
    {
        Debug.DrawLine(transform.position, transform.position + movement * 5, Color.cyan);

        RaycastHit hitInfo;
        if (Physics.BoxCast(m_collider.transform.position, m_collider.bounds.extents, movement.normalized, out hitInfo,
                            m_collider.transform.rotation, movement.magnitude, ~0, QueryTriggerInteraction.Ignore))
        {
            Debug.Log("Object hit: " + hitInfo.transform.name);

            Vector3 normal = hitInfo.normal;

            Vector3 newMovement = movement - (2 * Vector3.Dot(movement, normal) * normal);
            newMovement *= (m_collisionMomentumRetained / 100);
            newMovement.y = 0;

            // Multiplying velocity by delta time gets us movement.
            // Therefore, dividing movement by delta time gets us velocity.
            m_velocity = newMovement / Time.deltaTime;

            m_rigidbody.MovePosition(transform.position + newMovement);
        }
        else
        {
            m_rigidbody.MovePosition(transform.position + movement);
        }
    }

    /// <summary>
    /// Add the engine's force to velocity, until it has reached m_engineForce * decimalEnginePower
    /// </summary>
    private void ApplyEngine()
    {
        Vector3 localVelocity = transform.InverseTransformVector(m_velocity);

        float decimalEnginePower = m_enginePower / 100;
        float deltaEngine = decimalEnginePower * m_engineForce * Time.deltaTime;

        // If deltaEngine would increase velocity past the limit of m_engineForce * decimalEnginePower, then just set it to that limit
        if (localVelocity.z + deltaEngine > m_engineForce * decimalEnginePower)
        {
            localVelocity.z = m_engineForce * decimalEnginePower;
        }
        else
        {
            localVelocity.z += deltaEngine;
        }

        m_velocity = transform.TransformVector(localVelocity);
    }

    /// <summary>
    /// Apply sideways friction to velocity - forward friction is not needed
    /// </summary>
    private void ApplyFriction()
    {
        Vector3 localVelocity = transform.InverseTransformVector(m_velocity);

        float sideFriction = m_sideFriction * Time.deltaTime;

        if (Mathf.Abs(localVelocity.x) - sideFriction < 0)
        {
            localVelocity.x = 0;
        }
        else
        {
            localVelocity.x -= sideFriction * Mathf.Sign(localVelocity.x);
        }

        m_velocity = transform.TransformVector(localVelocity);
    }

    /// <summary>
    /// Clamp a vector3 based on its magnitude
    /// </summary>
    /// <param name="vector">Vector3 to clamp</param>
    /// <param name="maxMagnitude">Maximum magnitude before the vector is clamped</param>
    /// <returns></returns>
    private Vector3 ClampMagnitude3(Vector3 vector, float minMagnitude, float maxMagnitude)
    {
        if (vector.magnitude > maxMagnitude)
        {
            Vector3 deltaForce = vector - (vector.normalized * maxMagnitude);
            return vector - deltaForce;
        }
        if (vector.magnitude < minMagnitude)
        {
            Vector3 deltaForce = vector - (vector.normalized * minMagnitude);
            return vector - deltaForce;
        }

        return vector;
    }
}
