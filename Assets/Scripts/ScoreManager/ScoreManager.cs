using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private float m_Score;
    private const float INCREMENTED_SCORE = 5f;
    private const float DECREMENTED_SCORE = 1f;
    [SerializeField]
    private Text m_ScoreText;
    private const string SCORE_STRING = "SCORE: ";
    private const float INITIAL_SCORE = 0f;

    public float Score
    {
        get
        {
            return m_Score;
        }
    }
    private static ScoreManager s_ScoreManagerInstance = null;

    public static ScoreManager ScoreManagerInstance
    {
        get
        {
            return s_ScoreManagerInstance;
        }
    }

    void Awake()
    {
        m_Score = 0;
        if (s_ScoreManagerInstance == null)
        {
            s_ScoreManagerInstance = this;
            
        }else if(s_ScoreManagerInstance != this)
        {
            Destroy(gameObject);
        }
    }

    public void IncrementScore()
    {
        m_Score += INCREMENTED_SCORE;
        SetScore();
    }

    public void ResetScore()
    {
        m_Score = INITIAL_SCORE;
        SetScore();
    }

    public void DecrementScore()
    {
        m_Score -= DECREMENTED_SCORE;
        SetScore();
    }

    void SetScore()
    {
        m_ScoreText.text = SCORE_STRING + m_Score.ToString();
    }

}
