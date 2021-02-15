using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SoccerBall : MonoBehaviour
{
    [SerializeField] private Material m_landingMaterial;
    private GameObject m_landingDisplay = null;
    private BallisticTrajectoryRenderer m_trajectory;
    private Rigidbody m_rb = null;

    [SerializeField] private Vector3 m_vInitialVelocity = Vector3.zero;
    private Vector3 m_vInitialPosition;
    private Quaternion m_vInitialRotation;

    private bool m_bIsGrounded = true;
    private bool m_bHasScored = false;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        Assert.IsNotNull(m_rb, "Houston, we've got a problem! Rigidbody is not attached!");
        m_trajectory = GetComponent<BallisticTrajectoryRenderer>();

        // Create the landing display
        m_landingDisplay = new GameObject("Soccer Ball Landing Display");
        m_landingDisplay.transform.position = transform.position;
        m_landingDisplay.transform.localScale = transform.localScale;
        m_landingDisplay.AddComponent<MeshFilter>();
        m_landingDisplay.GetComponent<MeshFilter>().mesh = this.GetComponent<MeshFilter>().mesh;
        m_landingDisplay.AddComponent<MeshRenderer>();
        m_landingDisplay.GetComponent<MeshRenderer>().material = m_landingMaterial;

        m_vInitialPosition = transform.position;
        m_vInitialRotation = transform.rotation;
    }

    private void Update()
    {
        if (m_landingDisplay != null && m_bIsGrounded)
            m_landingDisplay.transform.position = GetLandingPosition();
 
        if (m_trajectory != null && m_bIsGrounded)
            m_trajectory.SetBallisticValues(transform.position, m_vInitialVelocity);
    }

    private Vector3 GetLandingPosition()
    {
        float fTime = 2f * (0f - m_vInitialVelocity.y / Physics.gravity.y);

        Vector3 vFlatVel = m_vInitialVelocity;
        vFlatVel.y = 0f;
        vFlatVel *= fTime;

        return transform.position + vFlatVel;
    }

    public bool HasScored()
    {
        return m_bHasScored;
    }

    #region CALLBACKS
    public void OnReset(bool fullReset)
    {
        m_bIsGrounded = true;
        m_bHasScored = false;
        if (fullReset)
        {
            transform.position = m_vInitialPosition;
            transform.rotation = m_vInitialRotation;
        }
    }
    public void OnKick()
    {
        if (!m_bIsGrounded)
        {
            return;
        }

        m_landingDisplay.transform.position = GetLandingPosition();
        m_bIsGrounded = false;

        transform.LookAt(m_landingDisplay.transform.position, Vector3.up);

        m_rb.velocity = m_vInitialVelocity;
    }
    public void OnScore()
    {
        m_bHasScored = true;
    }
    public void OnSetInitialVelocity(Vector3 value)
    {
        m_vInitialVelocity = value;
    }
    #endregion
}
