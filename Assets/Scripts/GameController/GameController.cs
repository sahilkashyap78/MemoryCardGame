using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_CardPrefab;
    [SerializeField]
    private int m_Rows = 4;
    [SerializeField]
    private int m_Columns = 3;
    [SerializeField]
    private Transform m_World;
    [SerializeField]
    InputManager m_InputManager;
    private float m_CardGap = 1f;
    private Vector3 m_DestinationPosition;
    Vector3 m_InitialPosition;
    private const float CARDWIDTH = 2.8f;
    private const float CARDHEIGHT =3.8f;
    private Action m_NextCardCallBack;
    private bool m_CanPutNextCard;

    public enum State
    {
        INITIALIZE,
        PLAY
    }
    private State m_State;
    public State currentState
    {
        get
        {
            return m_State;
        }
    }

    void OnEnable()
    {
        m_NextCardCallBack += CheckNextCardStatus;
    }

    void OnDisable()
    {
        m_NextCardCallBack -= CheckNextCardStatus;
    }

    void Awake()
    {
        m_State = State.INITIALIZE;
        m_DestinationPosition.x = m_World.position.x -(((float)m_Columns / 2 * (CARDWIDTH +  m_CardGap)) - (CARDWIDTH / 2f + m_CardGap / 2f));
        m_DestinationPosition.y = m_World.position.y + (((float)m_Rows / 2 * (CARDHEIGHT + m_CardGap)) - (CARDHEIGHT / 2f + m_CardGap / 2f));
        m_InitialPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height * -0.6f, 0f));
    }

    void Start()
    {
        StartCoroutine(DistributeCard());
    }

    void CheckNextCardStatus()
    {
        m_CanPutNextCard = true;
    }

    IEnumerator DistributeCard()
    {
        int tempCardId = 0;
        for (int rows = 0; rows < m_Rows; rows++)
        {
            for (int columns = 0; columns < m_Columns; columns++)
            {
                m_CanPutNextCard = false;
                tempCardId++;
                GameObject tempCard = Instantiate(m_CardPrefab, m_InitialPosition, Quaternion.identity, m_World);
                Card card = tempCard.GetComponent<Card>();
                card.Initialize(tempCardId, m_InitialPosition, m_DestinationPosition, m_NextCardCallBack);
                while (m_CanPutNextCard == false)
                {
                    yield return new WaitForEndOfFrame();
                }
                m_DestinationPosition.x += CARDWIDTH + m_CardGap;
            }
            m_DestinationPosition.x = m_World.position.x - (((float)m_Columns / 2 * (CARDWIDTH + m_CardGap)) - (CARDWIDTH /2f + m_CardGap / 2f));
            m_DestinationPosition.y -= CARDHEIGHT + m_CardGap;
        }
        CheckInputStatus(tempCardId);
    }

    private void CheckInputStatus(int id)
    {
        if (id >= m_Rows * m_Columns)
        {
            m_State = State.PLAY;
        }
    }
}
