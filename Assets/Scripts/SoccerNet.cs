using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

/// <summary>
/// Script for the overall soccer net.
/// The purpose of this script is only to notify the internal ScoreZone
/// about the GameManager for convenience of use.
/// </summary>
public class SoccerNet : MonoBehaviour
{
    [SerializeField] private GameManager m_gameManager;
    private void Start()
    {
        // Obtain a reference to the score zone and check that it's good.
        ScoreZone m_scoreZone = transform.Find("Score Zone").gameObject.GetComponent<ScoreZone>();
        Assert.IsNotNull(m_scoreZone, "Error! The SoccerNet does not have a ScoreZone registered!");

        // Set it's reference to the game manager.
        m_scoreZone.m_gameManager = m_gameManager;
    }
}