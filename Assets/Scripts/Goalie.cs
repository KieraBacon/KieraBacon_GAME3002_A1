using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for the soccer goalie.
/// Given two locations, the goalie will continually move back and forth between them.
/// </summary>
public class Goalie : MonoBehaviour
{
    [SerializeField] public Vector3 m_rightPos;
    [SerializeField] public Vector3 m_leftPos;
    [SerializeField] public float m_speed;
    void Update()
    {
        if (m_speed > 0.0f)
        {
            transform.position += (m_rightPos - transform.position).normalized * m_speed * Time.deltaTime;
            if ((m_rightPos - transform.position).sqrMagnitude < 0.01f)
                m_speed *= -1.0f;
        }
        else if (m_speed < 0.0f)
        {
            transform.position += (m_leftPos - transform.position).normalized * -m_speed * Time.deltaTime;
            if ((m_leftPos - transform.position).sqrMagnitude < 0.01f)
                m_speed *= -1.0f;
        }
    }
}
