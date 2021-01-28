using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    [SerializeField]
    private Text m_TimerText;
    private float m_Timer;
    private const string TIMER_STRING = " TIME: ";
    // Start is called before the first frame update
    void Awake()
    {
        m_Timer = 0f;
        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        while(m_Timer < 30f)
        {
            yield return new WaitForSeconds(1f);
            m_Timer++;
            m_TimerText.text = TIMER_STRING + m_Timer.ToString();
            yield return new WaitForEndOfFrame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
