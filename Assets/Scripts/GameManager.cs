using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_ScoreText = null;
    [SerializeField] private uint score = 0;

    public void Start()
    {
        if(m_ScoreText != null)
            m_ScoreText.text = "Score: " + score;
    }
    public void OnScore()
    {
        ++score;
        if (m_ScoreText != null)
            m_ScoreText.text = "Score: " + score;
    }
}
