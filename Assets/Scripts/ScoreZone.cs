using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    internal GameManager m_gameManager;

    private void OnTriggerEnter(Collider other)
    {
        SoccerBall ball = other.gameObject.GetComponent<SoccerBall>();
        if(ball != null && ball.HasScored() == false)
        {
            ball.OnScore();
            m_gameManager.OnScore();
        }
    }
}