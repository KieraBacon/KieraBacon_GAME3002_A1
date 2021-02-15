using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

/// <summary>
/// Script for the soccer ball itself.
/// This script is responsible for launching the ball,
/// and determining if the ball can score.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SoccerBall : MonoBehaviour
{   
    private Rigidbody m_rb;
    private Quaternion m_vInitialRotation; // Initial values to be reset to if a full reset is requested.
    private Vector3 m_vInitialPosition;    // ' '
    private Vector3 m_vKickVelocity; // Velocity to be applied when kicked.

    private bool m_bIsGrounded = true; // Ball can only be kicked when grounded.
    private bool m_bCanScore = true; // Ball can only score once before being reset.

    /// <summary>
    /// Set the initial values for the script.
    /// </summary>
    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        Assert.IsNotNull(m_rb, "Error! SoccerBall has no Rigidbody!");

        m_vInitialRotation = transform.rotation;
        m_vInitialPosition = transform.position;
        m_vKickVelocity = Vector3.zero;
    }

    /// <summary>
    /// Return whether or not the ball is capable of scoring.
    /// This should return false if the ball has recently scored,
    /// and has not been reset, so that it cannot score on a rebound from the net.
    /// </summary>
    public bool CanScore()
    {
        return m_bCanScore;
    }

    #region CALLBACKS
    /// <summary>
    /// Resets the ball so that it can be kicked again and can score again.
    /// </summary>
    /// <param name="fullReset">Setting this to true will also reset the position and rotation of the ball to their initial values.</param>
    public void OnReset(bool fullReset)
    {
        m_bIsGrounded = true;
        m_bCanScore = true;
        if (fullReset)
        {
            transform.position = m_vInitialPosition;
            transform.rotation = m_vInitialRotation;
        }
    }
    /// <summary>
    /// Kicks the ball using the velocity set by OnSetKickVelocity().
    /// </summary>
    public void OnKick()
    {
        if (!m_bIsGrounded) // Ball cannot be kicked if it's already in the air.
            return;

        // Determine the target position and face in that direction.
        Vector3 targetPosition = ProjectileBehaviour.GetLandingPosition(transform.position, m_vKickVelocity);
        transform.LookAt(targetPosition, Vector3.up);

        // Note that the ball is in the air, then actually make it so.
        m_bIsGrounded = false;
        m_rb.velocity = m_vKickVelocity;
    }

    /// <summary>
    /// Flags that the ball has scored since it's last reset,
    /// preventing it from doing so again.
    /// </summary>
    public void OnScore()
    {
        m_bCanScore = false;
    }

    /// <summary>
    /// Sets the velocity that the ball will be kicked with when it is kicked.
    /// </summary>
    public void OnSetKickVelocity(Vector3 value)
    {
        m_vKickVelocity = value;
    }
    #endregion
}
