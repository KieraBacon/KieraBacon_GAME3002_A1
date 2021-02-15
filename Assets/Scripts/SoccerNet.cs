using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

public class SoccerNet : MonoBehaviour
{
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private ScoreZone m_scoreZone;
    private void Start()
    {
        m_scoreZone.m_gameManager = m_gameManager;
    }
}