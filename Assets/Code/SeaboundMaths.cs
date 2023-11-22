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

    // From https://forum.unity.com/threads/debug-drawbox-function-is-direly-needed.1038499/
    public static void DrawBox(Vector3 pos, Quaternion rot, Vector3 scale, Color c)
    {
        // create matrix
        Matrix4x4 m = new Matrix4x4();
        m.SetTRS(pos, rot, scale);

        var point1 = m.MultiplyPoint(new Vector3(-0.5f, -0.5f, 0.5f));
        var point2 = m.MultiplyPoint(new Vector3(0.5f, -0.5f, 0.5f));
        var point3 = m.MultiplyPoint(new Vector3(0.5f, -0.5f, -0.5f));
        var point4 = m.MultiplyPoint(new Vector3(-0.5f, -0.5f, -0.5f));

        var point5 = m.MultiplyPoint(new Vector3(-0.5f, 0.5f, 0.5f));
        var point6 = m.MultiplyPoint(new Vector3(0.5f, 0.5f, 0.5f));
        var point7 = m.MultiplyPoint(new Vector3(0.5f, 0.5f, -0.5f));
        var point8 = m.MultiplyPoint(new Vector3(-0.5f, 0.5f, -0.5f));

        Debug.DrawLine(point1, point2, c);
        Debug.DrawLine(point2, point3, c);
        Debug.DrawLine(point3, point4, c);
        Debug.DrawLine(point4, point1, c);

        Debug.DrawLine(point5, point6, c);
        Debug.DrawLine(point6, point7, c);
        Debug.DrawLine(point7, point8, c);
        Debug.DrawLine(point8, point5, c);

        Debug.DrawLine(point1, point5, c);
        Debug.DrawLine(point2, point6, c);
        Debug.DrawLine(point3, point7, c);
        Debug.DrawLine(point4, point8, c);
    }
}
