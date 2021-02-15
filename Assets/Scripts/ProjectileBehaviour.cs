using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static utility functions for projectile behaviour.
/// </summary>
public static class ProjectileBehaviour
{
    /// <summary>
    /// Obtains the landing position of a projectile given its initial position and velocity
    /// assuming a flat surface and no drag.
    /// </summary>
    public static Vector3 GetLandingPosition(Vector3 position, Vector3 velocity)
    {
        // Obtain the time of flight.
        float fTime = 2f * (0f - velocity.y / Physics.gravity.y);

        // Get the horizontal velocity.
        Vector3 vFlatVel = velocity;
        vFlatVel.y = 0f;
        vFlatVel *= fTime;

        // Get the final position.
        return position + vFlatVel;
    }
}
