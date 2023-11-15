using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeAI : MonoBehaviour
{
    //[SerializeField]
    //GameObject m_player;
    //[SerializeField]
    //MovementController m_movementController;

    //[SerializeField]
    //float m_offsetFromPlayer;

    //void Update()
    //{
    //    RaycastHit hit;
    //    Physics.Raycast(transform.position, transform.forward, out hit, 60f);

    //    if (hit.transform != null)
    //    {
    //        TurnTowardPoint(-hit.transform.position);
    //    }
    //    else
    //    {
    //        float distToPlayer = (m_player.transform.position - transform.position).magnitude;
    //        if (distToPlayer < 80f)
    //        {
    //            TurnTowardPoint(-m_player.transform.position);
    //        }
    //        else
    //        {
    //            TurnTowardPoint(m_player.transform.position);
    //        }
    //    }

    //    m_movementController.AddVelocity(1);
    //}

    ///// <summary>
    ///// AI boat turns toward point if not within some angle
    ///// </summary>
    ///// <param name="point">Target point</param>
    //void TurnTowardPoint(Vector3 point)
    //{
    //    float angleToPoint = FindHeadingToPoint(point);

    //    if (!(angleToPoint < m_offsetFromPlayer && angleToPoint > -m_offsetFromPlayer))
    //    {
    //        m_movementController.Turn(Mathf.Sign(-angleToPoint));
    //    }
    //}

    ///// <summary>
    ///// Finds the heading from this object to some point in degrees
    ///// </summary>
    ///// <param name="point">Target point</param>
    ///// <returns>Angle toward point in degrees</returns>
    //float FindHeadingToPoint(Vector3 point)
    //{
    //    Vector2 forwardAngle = new(transform.forward.x, transform.forward.z);

    //    Vector3 directionToPoint = point - transform.position;
    //    Vector2 pointAngle = new(directionToPoint.x, directionToPoint.z);

    //    float angleToPoint = Vector2.SignedAngle(forwardAngle, pointAngle);
    //    return angleToPoint;
    //}
}
