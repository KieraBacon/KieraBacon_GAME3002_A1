using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Script for managing the state of the game.
/// In this case, that just means keeping track of the score.
/// Also in this case, it made sense to include the display of the score as well,
/// just for simplicity's sake.
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_ScoreText; // Reference to the score display text.
    [SerializeField] private uint score = 0;

    public void Start()
    {
        // Set the initial text for the score display.
        if(m_ScoreText != null)
            m_ScoreText.text = "Score: " + score;
    }
    public void OnScore()
    {
        // Increment the score and re-set the text display.
        ++score;
        if (m_ScoreText != null)
            m_ScoreText.text = "Score: " + score;
    }
}
