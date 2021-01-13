using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Card : MonoBehaviour
{
    [SerializeField]
    private GameController m_GameController;
    [SerializeField]
    private Sprite m_CardType;
    private SpriteRenderer m_SpriteRenderer;
    private int m_Id;
    private Vector3 m_InitialPosition;
    private Vector3 m_DestinationPosition;
    [SerializeField]
    private float m_MoveSpeed = 45f;
    private Sprite m_FaceSprite;
    [SerializeField]
    Sprite m_BackSprite;
    private Animator m_Animator;
    private bool m_FlipToFace;
    private bool m_FliptoBack;
    private bool m_Idle;
    private string m_Flip = "CANFLIP";
    private bool m_CanFlip;

    public bool Canflip
    {
        get
        {
            return m_CanFlip;
        }
    }

    public float id
    {
        get
        {
            return m_Id;
        }
    }

    private GameController.CardType m_CurrentType;

    public GameController.CardType CurrentType
    {
        get
        {
            return m_CurrentType;
        }
    }

    void Awake()
    {
        m_CanFlip = true;
        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
       
    }

    void Start()
    {
        Debug.Log(m_CurrentType);
    }

    public void FlipCard()
    {
        m_Animator.SetBool(m_Flip, m_CanFlip);
        if (m_CanFlip)
        {
            m_CanFlip = false;
        }
        else if(!m_CanFlip)
        {
            m_CanFlip = true;
        }
    }

    public void ChangeBackToFace()
    {
        m_SpriteRenderer.sprite = m_FaceSprite;
    }

    public void ChangeFaceToBack()
    {
        m_SpriteRenderer.sprite = m_BackSprite;
    }

    public void Initialize(int id, CardData cardData, Vector3 initialPosition, Vector3 destinationposition, Action nextCardCallBack)
    {
        //Sprite faceSprite, GameController.CardType currentType,
        m_Id = id;
        m_CurrentType = cardData.CardType;
        m_FaceSprite = cardData.FaceSprite;
        m_InitialPosition = initialPosition;
        m_DestinationPosition = destinationposition;
        StartCoroutine(MoveCard(nextCardCallBack));
    }

    private IEnumerator MoveCard(Action callBack)
    {
        float currentTime = 0f;
        float distance = Vector2.Distance(m_InitialPosition, m_DestinationPosition);
        float totalTime = distance / m_MoveSpeed;
        while (currentTime <= totalTime)
        {
            currentTime += Time.deltaTime;
            float interpolationPoint = currentTime / totalTime;
            transform.localPosition = Vector3.Lerp(m_InitialPosition, m_DestinationPosition, interpolationPoint);
            yield return new WaitForEndOfFrame();
        }
        callBack();
    }
}

