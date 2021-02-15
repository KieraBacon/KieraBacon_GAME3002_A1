using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for the area within a soccer net beyond which a goal has been scored.
/// </summary>
public class ScoreZone : MonoBehaviour
{
    internal GameManager m_gameManager; // The game manager to notify when a goal has been scored.
    private void OnTriggerEnter(Collider other)
    {
        // While anything can enter the net, only soccer balls can score a goal,
        // and only if that soccer ball has not already scored a goal in that net since it's last reset.
        SoccerBall ball = other.gameObject.GetComponent<SoccerBall>();
        if(ball != null && ball.CanScore())
        {
            ball.OnScore(); // Notify the ball that it can no longer score until it is reset.
            m_gameManager.OnScore(); // Notify the game manager that a goal has been scored.
        }
    }
}