using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    // --EDITOR VARIABLES--
    [SerializeField]
    Rigidbody m_rigidbody;
    [SerializeField]
    float m_maxVelocity;
    [SerializeField]
    float m_speed;
    [SerializeField]
    float m_brakeSpeed;
    [SerializeField]
    float m_maxRotationalVelocity;
    [SerializeField]
    float m_turnRate;
    [SerializeField]
    float m_friction;
    [SerializeField]
    float m_rotationalFriction;

    // --CODE VARIABLES--
    Vector2 m_velocity;

    float m_rotationalVelocity;
    float m_rotationEuler;

    // --UNITY METHODS--
    void Awake()
    {

    }

    void FixedUpdate()
    {
        // --VELOCITY CAP--
        // Cap rotation velocity
        // m_rotationVelocity can be positive or negative, so this is designed to be agnostic and apply the correct cap regardless
        if (Mathf.Abs(m_rotationalVelocity) > m_maxRotationalVelocity)
        {
            m_rotationalVelocity = Mathf.Sign(m_rotationalVelocity) * m_maxRotationalVelocity;
        }

        // Cap movement velocity
        m_velocity.y = Mathf.Clamp(m_velocity.y, 0, m_maxVelocity);

        // --FRICTION--
        // Apply turning friction
        m_rotationalVelocity = ApplyFriction(m_rotationalVelocity, m_rotationalFriction);

        // Apply movement friction
        m_velocity.x = ApplyFriction(m_velocity.x, m_friction);
        m_velocity.y = ApplyFriction(m_velocity.y, m_friction);


        // --MOVE PLAYER--
        // Rotate the player
        m_rotationEuler += m_rotationalVelocity;                              // Apply velocity to euler rotation
        m_rotationEuler %= 360;                                             // Stop euler from getting too big

        Quaternion rotation = Quaternion.Euler(0, m_rotationEuler, 0);      // Convert into quaternion
        m_rigidbody.MoveRotation(rotation);                                 // Apply to rigidbody

        // Move player forward
        m_rigidbody.MovePosition(transform.position + transform.forward * m_velocity.y * Time.deltaTime);
    }

    // --PUBLIC METHODS--
    /// <summary>
    /// Adds some velocity, controlled by the player's speed, to the object's current velocity.
    /// </summary>
    /// <param name="magnitude">The force by which speed is applied to the player's velocity.</param>
    public void AddVelocity(float magnitude = 1)
    {
        float velocity = magnitude * (magnitude > 0 ? m_speed : m_brakeSpeed);

        m_velocity.y += velocity * Time.deltaTime;
    }

    /// <summary>
    /// Adds some velocity, controlled by the player's speed, to the object's current turning velocity.
    /// </summary>
    /// <param name="magnitude">How sharply the boat should turn (-1 to 1).</param>
    public void Turn(float magnitude)
    {
        float eulerTurn = magnitude * m_turnRate * Time.deltaTime;

        m_rotationalVelocity += eulerTurn;
    }

    /// <summary>
    /// Brings a value closer to 0 by some amount determined by friction
    /// </summary>
    /// <param name="initialSpeed">The speed before friction is applied</param>
    /// <param name="friction">The amount to apply to initialSpeed</param>
    /// <returns>The speed after friction is applied</returns>
    private float ApplyFriction(float initialSpeed, float friction)
    {
        // Sign agnostic - if positive, this will subtract toward 0, if negative, it will add toward 0
        if (Mathf.Abs(initialSpeed) - (friction * Time.deltaTime) < 0)
        {
            return 0;
        }
        else
        {
            return initialSpeed - Mathf.Sign(initialSpeed) * friction * Time.deltaTime;
        }
    }
}
