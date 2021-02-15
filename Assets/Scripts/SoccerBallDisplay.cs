using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

/// <summary>
/// Script to display the trajectory and landing position of a soccer ball.
/// </summary>
public class SoccerBallDisplay : MonoBehaviour
{
    // Critical
    private Vector3 m_vInitialPosition; // The starting position of the ball.
    private Vector3 m_vKickVelocity; // The velocity with which the ball is expected to be kicked.

    // Trajectory
    private TrajectoryRenderer m_trajectory; // The line with which to display the ball's trajectory.
    private bool m_bDisplayTrajectory; // Whether or not to display the trajectory.

    // Landing Position
    [SerializeField] private GameObject m_templateObject; // The object to use as a template for drawing the ghost ball.
    [SerializeField] private Material m_landingMaterial; // The material for the landing display object. Having it be different from the soccer ball allows it to be transparent.
    private GameObject m_landingPosition; // The gameObject for the landing position ghost object.
    private bool m_bDisplayLandingPosition; // Whether or not to display the landing position.

    /// <summary>
    /// Make sure that the critical value - the soccer ball and its velocity - as well as the optional values are set up correctly.
    /// </summary>
    void Start()
    {
        m_vInitialPosition = Vector3.zero;
        m_vKickVelocity = Vector3.zero;

        m_trajectory = GetComponent<TrajectoryRenderer>();

        // Create the landing display object - if both possible and necessary
        if (m_templateObject != null && m_landingPosition == null)
        {
            m_landingPosition = new GameObject("Soccer Ball Landing Display");
            m_landingPosition.transform.position = m_templateObject.transform.position;
            m_landingPosition.transform.localScale = m_templateObject.transform.localScale;
            m_landingPosition.AddComponent<MeshFilter>();
            m_landingPosition.GetComponent<MeshFilter>().mesh = m_templateObject.GetComponent<MeshFilter>().mesh;
            m_landingPosition.AddComponent<MeshRenderer>();
            m_landingPosition.GetComponent<MeshRenderer>().material = m_landingMaterial;
        }
    }

    /// <summary>
    /// Update both the trajectory and landing position if both possible and desired.
    /// </summary>
    private void Update()
    {
        // The script should work fine even if there's no trajectory attached.
        if (m_trajectory != null)
        {
            // Only draw if requested by the controlling script.
            if (m_bDisplayTrajectory)
            {
                m_trajectory.SetInitialPositionAndVelocity(m_vInitialPosition, m_vKickVelocity);
                m_trajectory.OnSetShouldDraw(true);
            }
            else
            {
                m_trajectory.OnSetShouldDraw(false);
            }
        }

        // The script should work fine even if there's no landing position object attached.
        if (m_landingPosition != null)
        {
            // Only draw if requested by the controlling script.
            if (m_bDisplayLandingPosition)
            {
                m_landingPosition.SetActive(true);
                m_landingPosition.transform.position = ProjectileBehaviour.GetLandingPosition(m_vInitialPosition, m_vKickVelocity);
            }
            else
            {
                m_landingPosition.SetActive(false);
            }
        }
    }

    #region CALLBACKS
    public void OnSetKickValues(Vector3 position, Vector3 velocity)
    {
        m_vInitialPosition = position;
        m_vKickVelocity = velocity;
    }
    public void OnSetShowTrajectory(bool value)
    {
        m_bDisplayTrajectory = value;
    }
    public void OnSetShowLandingPosition(bool value)
    {
        m_bDisplayLandingPosition = value;
    }
    #endregion
}
