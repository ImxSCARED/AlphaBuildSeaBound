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
}
