using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

[RequireComponent(typeof(SoccerBall))]
public class SoccerBallController : MonoBehaviour
{
    private SoccerBall m_soccerBall = null;

    // Strength of the kick / magnitude of the resulting velocity
    private float m_fKickStrength = 0.0f;
    [SerializeField]
    private float m_fStrengthScaling = 2.0f;
    [SerializeField]
    public float m_fMaxStrength = 10.0f;
    [SerializeField]
    public float m_fMinStrength = 3.0f;

    // Angle of the kick / direction of the resulting velocity
    [SerializeField]
    private Quaternion m_fKickAngle = Quaternion.Euler(0.0f, 10.0f, 0.0f);
    [SerializeField]
    private float m_fAngleScalingX = 0.25f;
    [SerializeField]
    private float m_fAngleScalingY = 0.75f;
    [SerializeField]
    private float m_fKickAngleMinX = -45.0f;
    [SerializeField]
    private float m_fKickAngleMaxX = 45.0f;
    [SerializeField]
    private float m_fKickAngleMinY = 10.0f;
    [SerializeField]
    private float m_fKickAngleMaxY = 90.0f;

    void Start()
    {
        m_soccerBall = GetComponent<SoccerBall>();
        Assert.IsNotNull(m_soccerBall, "Error! The Soccer Ball Controller has no Soccer Ball!");
    }

    void Update()
    {
        HandleUserInput();
    }

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
            else if (m_fKickStrength < 0.0f)
            {
                m_fStrengthScaling = Mathf.Abs(m_fStrengthScaling);
                m_fKickStrength = 0.0f + Time.deltaTime * m_fStrengthScaling;
            }

            // Adjust the angle of the kick based on mouse movement
            m_fKickAngle.x += Input.GetAxis("Mouse X") * m_fAngleScalingX;
            m_fKickAngle.y += Input.GetAxis("Mouse Y") * m_fAngleScalingY;
            m_fKickAngle.x = Mathf.Clamp(m_fKickAngle.x, m_fKickAngleMinX, m_fKickAngleMaxX);
            m_fKickAngle.y = Mathf.Clamp(m_fKickAngle.y, m_fKickAngleMinY, m_fKickAngleMaxY);

            // Obtain a direction vector from the KickAngle Quaternion
            Vector3 direction = new Vector3(Mathf.Sin(m_fKickAngle.x * Mathf.Deg2Rad),
                                            m_fKickAngle.y * Mathf.Deg2Rad,
                                            Mathf.Cos(m_fKickAngle.x * Mathf.Deg2Rad));

            // Pass the results to the soccer ball
            m_soccerBall.OnSetInitialVelocity(direction.normalized * m_fKickStrength);
        }
        else if (m_fKickStrength > 0.0f)
        {
            m_soccerBall.OnKick();
            m_fKickStrength = 0.0f;
        }
    }
}
