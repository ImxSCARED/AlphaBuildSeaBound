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
    [Tooltip("Maximum forward speed that the player can reach")]
    [Min(0f)]
    float m_maxSpeed;
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
    [Tooltip("Rate at which the boat slows in the forward and backward directions")]
    [Min(0f)]
    float m_forwardFriction;
    [SerializeField]
    [Tooltip("Rate at which the boat slows in the horizontal directions")]
    [Min(0f)]
    float m_sideFriction;

    // Collision
    [SerializeField]
    [Tooltip("Percentage of momentum maintained in a collision")]
    [Range(0f, 100f)]
    float m_collisionMomentumRetained;
    [SerializeField]
    [Tooltip("Layers that the boat can collide with")]
    LayerMask m_collisionLayerMask;

    // --CODE VARIABLES--

    // Velocity
    Vector3 m_velocity = Vector3.zero;
    float m_forwardImpulse = 0f;

    // Rotation
    Quaternion m_amountToRotate = Quaternion.identity;

    // Collision
    Collider[] m_childColliders;

    void Start()
    {
        if (m_maxVelocity < m_maxSpeed)
        {
            m_maxVelocity = m_maxSpeed;
        }

        // Set m_childColliders
        Collider[] allColliders = GetComponentsInChildren<Collider>();
        m_childColliders = new Collider[allColliders.Length - 1];

        int subtractFromIndex = 0;
        for (int i = 0; i < allColliders.Length; i++)
        {
            // If this is not our collider...
            if (allColliders[i] != GetComponent<Collider>())
            {
                //...then add it to the child list
                m_childColliders[i - subtractFromIndex] = allColliders[i];
            }
            else
            {
                // If it is, then we need to offset our index so we don't overflow the new array
                subtractFromIndex = 1;
            }
        }
    }

    void FixedUpdate()
    {
        // Setup
        IgnoreChildColliders();

        // Player speed
        ApplySpeedImpulse(m_forwardImpulse);

        // Clamp velocity
        m_velocity = SeaboundMaths.ClampMagnitude3(m_velocity, -m_maxVelocity, m_maxVelocity);
        m_velocity.y = 0;

        // Apply friction
        ApplyFriction();

        // Apply velocity
        MoveWithCollision(m_velocity * Time.deltaTime);
        RotateWithCollision(m_amountToRotate);

        Debug.Log("Is colliding: " + Physics.CheckBox(m_rigidbody.transform.position, m_collider.size / 2, m_rigidbody.rotation, m_collisionLayerMask, QueryTriggerInteraction.Ignore));

        // Cleanup
        m_forwardImpulse = 0f;
        m_amountToRotate = Quaternion.identity;
        UnignoreChildColliders();
    }

    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(m_collider.transform.position, m_collider.size);
    }

    /// <summary>
    /// Adds a vector3 force to velocity, mostly used internally
    /// </summary>
    /// <param name="force">Force to apply</param>
    public void AddForce(Vector3 force)
    {
        m_velocity += force * Time.fixedDeltaTime;
    }

    /// <summary>
    /// Causes the engine to accelerate by magnitude * m_accelerationSpeed (Time.deltaTime is applied later)
    /// </summary>
    /// <param name="magnitude">Amount from -1 to 1 that the engine should accelerate</param>
    public void Accelerate(float magnitude)
    {
        magnitude = Mathf.Clamp(magnitude, -1f, 1f);

        float deltaEngine = magnitude * m_accelerationSpeed;
        m_forwardImpulse = deltaEngine;
    }

    /// <summary>
    /// Rotates the boat by magnitude * m_turnSpeed (Time.deltaTime is applied later)
    /// TODO: make this rotate around custom axis
    /// TODO: ensure the turning works properly with speed
    /// </summary>
    /// <param name="magnitude">Amount from -1 to 1 that the boat should rotate</param>
    public void Turn(float magnitude)
    {
        magnitude = Mathf.Clamp(magnitude, -1f, 1f);

        float deltaTurn = magnitude * m_turnSpeed;

        m_amountToRotate = Quaternion.Euler(0f, deltaTurn * Mathf.Deg2Rad, 0f);
    }

    public void MoveWithCollision(Vector3 movement)
    {
        if (m_rigidbody.SweepTest(movement.normalized, out RaycastHit hitInfo, movement.magnitude, QueryTriggerInteraction.Ignore))
        {
            Debug.Log("Object hit: " + hitInfo.transform.name);

            // Get collision normal
            Vector3 normal = hitInfo.normal;

            // Find reflected movement vector and scale it by m_collisionMomentumRetained
            Vector3 newMovement = movement - (2 * Vector3.Dot(movement, normal) * normal);
            newMovement *= (m_collisionMomentumRetained / 100);
            newMovement.y = 0;

            // Multiplying velocity by delta time gets us movement.
            // Therefore, dividing movement by delta time gets us velocity.
            m_velocity = newMovement / Time.deltaTime;

            // Get point hit, offset by the distance from a point to the centre of the ship
            Vector3 pointHit = new Vector3(hitInfo.point.x, m_rigidbody.transform.position.y, hitInfo.point.z);
            Vector3 directionToShip = m_rigidbody.transform.position - pointHit;


            m_rigidbody.MovePosition(m_rigidbody.transform.position + newMovement);
        }
        else
        {
            m_rigidbody.MovePosition(m_rigidbody.transform.position + movement);
        }
    }

    public void RotateWithCollision(Quaternion rotation)
    {
        Quaternion newRotation = m_rigidbody.rotation * rotation;

        // If our rotation doesn't collide with anything...
        if (!Physics.CheckBox(m_rigidbody.transform.position, m_collider.size / 2, newRotation, m_collisionLayerMask, QueryTriggerInteraction.Ignore))
        {
            //... then we should rotate
            m_rigidbody.MoveRotation(newRotation);

            float decimalPercentTVR = m_turningVelocityRetained / 100;
            m_velocity = (transform.forward * m_velocity.magnitude * decimalPercentTVR) + (m_velocity* (1 - decimalPercentTVR));
        }
    }

    /// <summary>
    /// Add the engine's force to velocity, until it has reached m_engineForce * decimalEnginePower
    /// </summary>
    private void ApplySpeedImpulse(float deltaEngine)
    {
        Vector3 localVelocity = transform.InverseTransformVector(m_velocity);

        deltaEngine *= Time.deltaTime;

        // Probably a nicer way to do this
        // If deltaEngine would increase velocity past the limit of m_engineForce * decimalEnginePower, then just set it to that limit
        if (localVelocity.z + deltaEngine > m_maxSpeed)
        {
            localVelocity.z = m_maxSpeed;
        }
        //; Same as above, but minimum with 0
        else if (localVelocity.z + deltaEngine < 0)
        {
            localVelocity.z = 0;
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

        float forwardFriction = m_forwardFriction * Time.deltaTime;
        float sideFriction = m_sideFriction * Time.deltaTime;

        localVelocity.z = SeaboundMaths.ApplyFriction(localVelocity.z, forwardFriction);
        localVelocity.x = SeaboundMaths.ApplyFriction(localVelocity.x, sideFriction);

        m_velocity = transform.TransformVector(localVelocity);
    }

    private void IgnoreChildColliders()
    {
        foreach (Collider col in m_childColliders)
        {
            col.enabled = false;
        }
    }

    private void UnignoreChildColliders()
    {
        foreach (Collider col in m_childColliders)
        {
            col.enabled = true;
        }
    }
}
