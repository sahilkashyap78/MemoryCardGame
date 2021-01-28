using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopUp : MonoBehaviour
{
    [SerializeField]
    private GameController m_GameController;
    [SerializeField]
    private Text m_ResultText;
    [SerializeField]
    private Text m_ScoreText;
    [SerializeField]
    private Text m_PlayAgainButtonText;
    private const string SCORE_STRING = "Score:";
    private const string WIN_STRING = "You Won! ";
    private const string WIN_BUTTONTEXT = "PlayAgain";
    private const string LOSE_BUTTONTEXT = "Retry";
    private const string LOSE_STRING = "You Lose! ";
    private const bool ACTIVE_POPUP = true;
    private const bool DEACTIVE_POPUP = false;
    private const string SCALE_POPUP = "ScalePopUp";
    private const string ZOOM_POPUP = "ZoomPopUp";
    private bool m_CanScalePopUp;
    private Animator m_Animator;

    void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_CanScalePopUp = false;
    }

    public void ScalePopUp()
    {
        m_CanScalePopUp = true;
        m_Animator.SetBool(SCALE_POPUP, m_CanScalePopUp);
    }

    public void DeactivePopUp()
    {
        gameObject.SetActive(DEACTIVE_POPUP);
    }

    public void ShowPopUp()
    {
        bool zoomPopUp = true;
        gameObject.SetActive(ACTIVE_POPUP);
        m_Animator.SetBool(ZOOM_POPUP, zoomPopUp);
        if (m_GameController.currentState == GameController.State.WIN)
        {
            m_ResultText.text = WIN_STRING;
            m_PlayAgainButtonText.text = WIN_BUTTONTEXT;
        }
        else if(m_GameController.currentState == GameController.State.LOSE)
        {
            m_ResultText.text = LOSE_STRING;
            m_PlayAgainButtonText.text = LOSE_BUTTONTEXT;
        }
        m_ScoreText.text = SCORE_STRING + ScoreManager.ScoreManagerInstance.Score;
    }
}
