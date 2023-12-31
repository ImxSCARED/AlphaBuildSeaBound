using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
    CapsuleCollider m_collider;
    [SerializeField]
    GameObject m_wheels;
    [SerializeField]
    GameObject m_geometryParent;
    [SerializeField]
    AudioManager m_audioManager;
    [SerializeField]
    Animator shipSailsAnimator;

    [Header("Player Control")]

    // Acceleration
    [SerializeField]
    [Tooltip("Maximum forward speed that the player can reach")]
    [Min(0f)]
    float m_maxSpeed;
    [SerializeField]
    [Tooltip("Maximum forward speed that the player can reach")]
    [Min(0f)]
    float m_maxBackwardSpeed;
    [SerializeField]
    [Tooltip("Speed at which the ship accelerates to max speed per second in percentages (e.g. 200% per second)")]
    [Min(0f)]
    float m_accelerationSpeed;

    // Turning
    [SerializeField]
    [Tooltip("Rate at which ship turning accelerates per second in degrees. Increase to account for turn friction")]
    [Min(0f)]
    float m_turnSpeed;
    [SerializeField]
    [Tooltip("Speed at which the boat tilts while turning")]
    [Min(0f)]
    float m_tiltSpeed;
    [SerializeField]
    [Tooltip("Maximum angle in degrees that the boat can tilt while turning")]
    [Min(0f)]
    float m_maxTilt;


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

    // Wheel Spin Object Reference

    // --CODE VARIABLES--

    // Velocity
    Vector3 m_velocity = Vector3.zero;
    float m_forwardImpulse = 0f;

    // Rotation
    Quaternion m_amountToRotate = Quaternion.identity;
    float m_amountToTilt;
    float m_targetTilt;

    //Upgrade
    public float m_upgradeAmount = 1;

    void Start()
    {
        if (m_maxVelocity < m_maxSpeed * m_upgradeAmount)
        {
            m_maxVelocity = m_maxSpeed * m_upgradeAmount;
        }
    }

    void FixedUpdate()
    {
        // Setup

        // Player speed
        ApplySpeedImpulse(m_forwardImpulse);

        // Clamp velocity
        m_velocity = SeaboundMaths.ClampMagnitude3(m_velocity, -m_maxVelocity, m_maxVelocity);
        m_velocity.y = 0;

        // Apply friction
        ApplySideFriction();

        // Apply velocity
        MoveWithCollision(m_velocity * Time.deltaTime);
        RotateWithCollision(m_amountToRotate);

        // Sound
        MovementSound(m_velocity.magnitude / m_maxVelocity);

        

        // Cleanup
        m_forwardImpulse = 0f;
        m_amountToRotate = Quaternion.identity;
    }

    private void Update()
    {
        // Spin Wheel
        m_wheels.transform.Rotate(m_velocity.magnitude * Time.deltaTime * 10, 0, 0f, Space.Self);

        // Tilt
        // It would be preferred if this could be done in an animation instead
        if (m_amountToTilt != 0)
        {
            m_targetTilt += m_amountToTilt * Time.deltaTime;

            // Multiply by percentage of max speed to base the tilting around movement speed
            float speedFraction = m_velocity.magnitude / m_maxVelocity;
            // Multiply by amount of stick tilt to base the tilting around the left stick
            float stickFraction = Mathf.Abs(m_amountToTilt / m_tiltSpeed);
            m_targetTilt = Mathf.Clamp(m_targetTilt, -m_maxTilt * speedFraction * stickFraction, m_maxTilt * speedFraction * stickFraction);
        }
        else
        {
            // Apply friction is kinda abstract here, but the function returns it to 0 at some rate (similar to lerping, actually) so it works
            m_targetTilt = SeaboundMaths.ApplyFriction(m_targetTilt, m_turnSpeed * Time.deltaTime);
        }

        m_geometryParent.transform.localRotation = Quaternion.Euler(0, 0, m_targetTilt);

        // Cleanup
        // Reset this back to 0 so that we can check if we're turning on a given frame
        m_amountToTilt = 0;
    }

    private void OnDrawGizmos()
    {
        Vector3 startPoint = transform.position + m_collider.center - (m_collider.transform.forward * (m_collider.height / 2));
        Vector3 endPoint = startPoint + (m_collider.transform.forward * m_collider.height);

        Gizmos.DrawWireCube(transform.position + m_collider.center, new Vector3(m_collider.radius * 2, m_collider.radius * 2, m_collider.height - m_collider.radius * 2));
        Gizmos.DrawWireSphere(startPoint + m_collider.transform.forward * m_collider.radius, m_collider.radius);
        Gizmos.DrawWireSphere(endPoint - m_collider.transform.forward * m_collider.radius, m_collider.radius);
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

        float deltaEngine = magnitude * (m_accelerationSpeed * m_upgradeAmount);
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

        float deltaTurn = magnitude * (m_turnSpeed * m_upgradeAmount);

        m_amountToRotate = Quaternion.Euler(0f, deltaTurn * Mathf.Deg2Rad, 0f);

        m_amountToTilt = -magnitude * m_tiltSpeed;
    }

    public void MoveWithCollision(Vector3 movement)
    {
        if (movement != Vector3.zero)
        {
            Vector3 startPoint = transform.position + m_collider.center - (m_collider.transform.forward * (m_collider.height / 2));
            Vector3 endPoint = startPoint + (m_collider.transform.forward * m_collider.height);

            if (Physics.CapsuleCast(startPoint, endPoint, m_collider.radius, movement.normalized, out RaycastHit hitInfo,
                                    movement.magnitude, m_collisionLayerMask, QueryTriggerInteraction.Ignore))
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


                m_rigidbody.MovePosition(m_rigidbody.position + newMovement);
            }
            else if (Physics.CheckCapsule(startPoint, endPoint, m_collider.radius, m_collisionLayerMask, QueryTriggerInteraction.Ignore))
            {
                m_velocity = -movement / Time.deltaTime;
                m_rigidbody.MovePosition(m_rigidbody.position - movement);
            }
            else
            {
                m_rigidbody.MovePosition(m_rigidbody.position + movement);
            }
        }
    }

    public void RotateWithCollision(Quaternion rotation)
    {
        if (rotation != Quaternion.identity)
        {
            Quaternion newRotation = m_rigidbody.rotation * rotation;

            Vector3 newForward = rotation * m_collider.transform.forward;
            Vector3 startPoint = transform.position + m_collider.center - (newForward * (m_collider.height / 2));
            Vector3 endPoint = startPoint + (newForward * m_collider.height);

            // If our rotation doesn't collide with anything...
            if (!Physics.CheckCapsule(startPoint, endPoint, m_collider.radius, m_collisionLayerMask, QueryTriggerInteraction.Ignore))
            {
                //... then we should rotate
                m_rigidbody.MoveRotation(newRotation);

                float decimalPercentTVR = m_turningVelocityRetained / 100;
                m_velocity = (transform.forward * m_velocity.magnitude * decimalPercentTVR) + (m_velocity * (1 - decimalPercentTVR));
            }
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
        if (localVelocity.z + deltaEngine > m_maxSpeed * m_upgradeAmount)
        {
            localVelocity.z = m_maxSpeed * m_upgradeAmount;
        }
        //; Same as above, but minimum with 0
        else if (localVelocity.z + deltaEngine < -m_maxBackwardSpeed)
        {
            localVelocity.z = -m_maxBackwardSpeed;
        }
        else
        {
            localVelocity.z += deltaEngine;
        }

        // Animator
        shipSailsAnimator.SetLayerWeight(1, Mathf.Abs(localVelocity.z / m_maxSpeed * m_upgradeAmount));
        shipSailsAnimator.SetLayerWeight(2, Mathf.Abs(localVelocity.z / m_maxSpeed * m_upgradeAmount));

        m_velocity = transform.TransformVector(localVelocity);
    }

    /// <summary>
    /// Apply sideways friction to velocity - forward friction is not needed
    /// </summary>
    private void ApplySideFriction()
    {
        Vector3 localVelocity = transform.InverseTransformVector(m_velocity);

        //float forwardFriction = m_forwardFriction * Time.deltaTime;
        float sideFriction = m_sideFriction * Time.deltaTime;

        //localVelocity.z = SeaboundMaths.ApplyFriction(localVelocity.z, forwardFriction);
        localVelocity.x = SeaboundMaths.ApplyFriction(localVelocity.x, sideFriction);

        m_velocity = transform.TransformVector(localVelocity);
    }

    public void StopMovement()
    {
        m_forwardImpulse = 0;
        m_velocity = Vector3.zero;
    }

    // Myles' stuff
    private void MovementSound(float percentMaxVelocity)
    {
        //if (percentMaxVelocity > 0.2)
        //{
        //    AudioManager.instance.PlayNextClip("Sailing1");
        //}
        //else if (percentMaxVelocity <= 0.4)
        //{
        //    AudioManager.instance.PlayNextClip("Sailing2");
        //}
        //else if (percentMaxVelocity <= 0.6)
        //{
        //    AudioManager.instance.PlayNextClip("Sailing3");
        //}
        //else if (percentMaxVelocity <= 0.8)
        //{
        //    AudioManager.instance.PlayNextClip("Sailing4");
        //}
        //else if (percentMaxVelocity <= 1)
        //{
        //    AudioManager.instance.PlayNextClip("Sailing5");
        //}
    }
}
