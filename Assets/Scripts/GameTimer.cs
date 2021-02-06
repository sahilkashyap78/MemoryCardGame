using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameTimer : MonoBehaviour
{
    private const float GREEN_RANGE = 30f;
    private const float YELLOW_RANGE = 20f;
    private const float RED_RANGE = 10f;
    private const float BLINK_RANGE = 5f;
    private const float RESET_TIMER = 30f;
    private const float MIN_TIME = 0f;
    private const float WAITING_TIME = 1f;
    private float m_Timer;
    [SerializeField]
    private Text m_TimerText;
    private Animator m_GameTimerAnimator;
    private bool m_BlinkTimer;
    private Text m_GameTimerText;
    private const string CAN_BLINK = "CanBlink";
    private GameController.State m_CurrentState;
    [SerializeField]
    GameController m_GameController;
    [SerializeField]
    GameOverPopUp m_PopUp;
    public bool blinkTimer
    {
        get
        {
            return m_BlinkTimer;
        }
    }

    void Awake()
    {
        m_BlinkTimer = false;
        m_Timer = RESET_TIMER;
        m_GameTimerAnimator = m_TimerText.GetComponent<Animator>();
        m_GameTimerText = m_TimerText.GetComponent<Text>();
    }


    public IEnumerator Timer()
    {
        while (m_Timer > MIN_TIME)
        {
            SetTimerColor();
            yield return new WaitForSeconds(WAITING_TIME);
            m_Timer--;
            m_TimerText.text = m_Timer.ToString();
        }

        m_GameController.currentState = GameController.State.LOSE;
        BlinkTimer();
        m_PopUp.ShowPopUp();
    }

    public void ResetTimer()
    {
        m_Timer = RESET_TIMER;
    }

    public void BlinkTimer()
    {
        m_BlinkTimer = !m_BlinkTimer;
        m_GameTimerAnimator.SetBool(CAN_BLINK, m_BlinkTimer);
    }


    private void SetTimerColor()
    {
        if (m_Timer == YELLOW_RANGE)
        {
            m_GameTimerText.color = Color.yellow;
        }
        else if (m_Timer == RED_RANGE)
        {
            m_GameTimerText.color = Color.red;
        }
        else if (m_Timer == BLINK_RANGE)
        {
            BlinkTimer();
        }
    }

}












//accelerator wire
//break liner (piche ke)
//Service