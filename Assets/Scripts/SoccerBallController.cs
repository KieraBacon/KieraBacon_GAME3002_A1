using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

/// <summary>
/// Script for user input and control of the soccer ball.
/// Pressing and holding the Left Mouse Button will increase the strength of the kick
/// up to a maximum, and then decrease it at the same rate down to a minimum.
/// Moving the mouse horizontally will change the (horizontal) direction of the kick,
/// and moving it vertically will change the (vertical) angle of the kick, 
/// thus also increasing the range.
/// Releasing the Left Mouse Button after holding it will kick the ball.
/// </summary>
public class SoccerBallController : MonoBehaviour
{
    // References to the soccer ball being controlled and it's display object.
    [SerializeField] private SoccerBall m_soccerBall;
    [SerializeField] private SoccerBallDisplay m_soccerBallDisplay;

    // Strength of the kick / magnitude of the resulting velocity.
    private float m_fKickStrength;
    [SerializeField] private float m_fStrengthScaling = 5.0f; // Speed of increase/decrease for kick strength.
    [SerializeField] public float m_fMaxStrength = 10.0f;
    [SerializeField] public float m_fMinStrength = 3.0f;

    // Angle of the kick / direction of the resulting velocity.
    [SerializeField] private Quaternion m_fKickAngle;
    [SerializeField] private float m_fAngleScalingX = 0.25f; // Speed at which moving the mouse horizontally changes kick direction.
    [SerializeField] private float m_fAngleScalingY = 0.75f; // Speed at which moving the mouse vertically changes kick angle.
    [SerializeField] public float m_fKickAngleMinX = -45.0f;
    [SerializeField] public float m_fKickAngleMaxX = 45.0f;
    [SerializeField] public float m_fKickAngleMinY = 10.0f;
    [SerializeField] public float m_fKickAngleMaxY = 90.0f;

    void Start()
    {
        // Make sure that the controller is controlling a ball.
        if(m_soccerBall == null)
            m_soccerBall = GetComponent<SoccerBall>();
        Assert.IsNotNull(m_soccerBall, "Error! The SoccerBallController has no SoccerBall!");

        // Set the initial kick strength to the min.
        Reset();
    }

    /// <summary>
    /// Handle user input on update.
    /// </summary>
    void Update()
    {
        HandleUserInput();
    }

    /// <summary>
    /// Reset the kick strength and angle to their default values.
    /// </summary>
    private void Reset()
    {
        m_fKickStrength = m_fMinStrength;
        m_fKickAngle = Quaternion.Euler(0.0f, m_fKickAngleMinY, 0.0f);
    }

    /// <summary>
    /// User input is the Left Mouse Button as well as the mouse position.
    /// </summary>
    void HandleUserInput()
    {
        if (Input.GetAxis("Fire1") > 0.0f)
        {
            m_soccerBall.OnReset(true);

            // Add or subtract to the kick strength based on deltaTime,
            // then Limit the kick strength between the max and the min
            // by adjusting the direction of change.
            m_fKickStrength += Time.deltaTime * m_fStrengthScaling;
            if (m_fKickStrength >= m_fMaxStrength)
            {
                m_fStrengthScaling = -Mathf.Abs(m_fStrengthScaling);
                m_fKickStrength = m_fMaxStrength + Time.deltaTime * m_fStrengthScaling;
            }
            else if (m_fKickStrength < m_fMinStrength)
            {
                m_fStrengthScaling = Mathf.Abs(m_fStrengthScaling);
                m_fKickStrength = m_fMinStrength + Time.deltaTime * m_fStrengthScaling;
            }

            // Adjust the kick angle based on mouse movement.
            m_fKickAngle.x += Input.GetAxis("Mouse X") * m_fAngleScalingX;
            m_fKickAngle.y += Input.GetAxis("Mouse Y") * m_fAngleScalingY;
            
            // Clamp the kick angle to within the specified limits.
            m_fKickAngle.x = Mathf.Clamp(m_fKickAngle.x, m_fKickAngleMinX, m_fKickAngleMaxX);
            m_fKickAngle.y = Mathf.Clamp(m_fKickAngle.y, m_fKickAngleMinY, m_fKickAngleMaxY);

            // Obtain the new direction vector from the KickAngle Quaternion.
            Vector3 direction = new Vector3(Mathf.Sin(m_fKickAngle.x * Mathf.Deg2Rad),
                                            m_fKickAngle.y * Mathf.Deg2Rad,
                                            Mathf.Cos(m_fKickAngle.x * Mathf.Deg2Rad));

            // Pass the results to the soccer ball and soccer ball display objects.
            Vector3 kickVelocity = direction.normalized * m_fKickStrength;
            m_soccerBall.OnSetKickVelocity(kickVelocity);
            m_soccerBallDisplay.OnSetKickValues(m_soccerBall.transform.position, kickVelocity);
            m_soccerBallDisplay.OnSetShowTrajectory(true);
            m_soccerBallDisplay.OnSetShowLandingPosition(true);
        }

        // If the "Fire1" axis is not > 0, it means the player has released the button,
        // hence, kick it!
        else if (m_fKickStrength > m_fMinStrength)
        {
            m_soccerBall.OnKick(); // Launch the soccer ball!
            Reset();
            m_soccerBallDisplay.OnSetShowTrajectory(false); // Trajectory should only be displayed while holding the button.
            m_soccerBallDisplay.OnSetShowLandingPosition(true); // But the landing position can be displayed always.
        }
    }
}
