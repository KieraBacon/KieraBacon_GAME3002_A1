using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SoccerBall : MonoBehaviour
{
    [SerializeField] private Vector3 m_vInitialVelocity = Vector3.zero;
    [SerializeField] private Material m_landingMaterial;
    private Rigidbody m_rb = null;
    private GameObject m_landingDisplay = null;
    private bool m_bIsGrounded = true;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        Assert.IsNotNull(m_rb, "Houston, we've got a problem! Rigidbody is not attached!");

        // Create the landing display
        m_landingDisplay = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        m_landingDisplay.transform.position = Vector3.zero;
        m_landingDisplay.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        m_landingDisplay.GetComponent<Renderer>().material = m_landingMaterial;
        m_landingDisplay.GetComponent<Collider>().enabled = false;
    }

    private void Update()
    {
        if (m_landingDisplay != null && m_bIsGrounded)
        {
            m_landingDisplay.transform.position = GetLandingPosition();
        }
    }

    private Vector3 GetLandingPosition()
    {
        float fTime = 2f * (0f - m_vInitialVelocity.y / Physics.gravity.y);

        Vector3 vFlatVel = m_vInitialVelocity;
        vFlatVel.y = 0f;
        vFlatVel *= fTime;

        return transform.position + vFlatVel;
    }

    #region CALLBACKS
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

    public void OnMoveForward(float fDelta)
    {
        m_vInitialVelocity.z += fDelta;
    }

    public void OnMoveBackward(float fDelta)
    {
        m_vInitialVelocity.z -= fDelta;
    }

    public void OnMoveRight(float fDelta)
    {
        m_vInitialVelocity.x += fDelta;
    }

    public void OnMoveLeft(float fDelta)
    {
        m_vInitialVelocity.x -= fDelta;
    }
    #endregion
}
