using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SeaboundMaths
{
    /// <summary>
    /// Applies friction to a speed float
    /// </summary>
    /// <param name="originalSpeed">Speed to apply friction to</param>
    /// <param name="friction">Rate at which speed slows</param>
    /// <returns>Speed after friction</returns>
    public static float ApplyFriction(float originalSpeed, float friction)
    {
        if (Mathf.Abs(originalSpeed) - friction < 0)
        {
            return 0;
        }
        else
        {
            return originalSpeed - friction * Mathf.Sign(originalSpeed);
        }
    }

    /// <summary>
    /// Clamp a vector3 based on its magnitude
    /// </summary>
    /// <param name="vector">Vector3 to clamp</param>
    /// <param name="maxMagnitude">Maximum magnitude before the vector is clamped</param>
    /// <returns>Clamped Vector3</returns>
    public static Vector3 ClampMagnitude3(Vector3 vector, float minMagnitude, float maxMagnitude)
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
