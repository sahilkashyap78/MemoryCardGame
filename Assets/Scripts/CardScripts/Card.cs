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
    private const string CAN_SCALE = "CANSCALE";
    private const string IDLE_STATE = "IDLE";
    private bool m_CanScale;

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
        m_CanScale = true;
        m_CanFlip = true;
        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
       
    }

    public void FlipCard()
    {
        m_Animator.SetBool(m_Flip, m_CanFlip);
        m_CanFlip = !m_CanFlip;
    }

    public void ScaleCard()
    {
        m_Animator.SetTrigger("SCALE");
        //m_Animator.SetBool(CAN_SCALE, m_CanScale);
        //m_CanScale = !m_CanScale;
    }

    public void MoveToIdle()
    {
        m_Animator.SetTrigger(IDLE_STATE);
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
        m_Id = id;
        m_CurrentType = cardData.CardType;
        m_FaceSprite = cardData.FaceSprite;
        StartCoroutine(MoveCard(initialPosition, destinationposition, nextCardCallBack));
    }

    public IEnumerator MoveCard(Vector3 initialPosition, Vector3 destinationPosition, Action callBack)
    {
        float currentTime = 0f;
        float distance = Vector2.Distance(initialPosition, destinationPosition);
        float totalTime = distance / m_MoveSpeed;
        while (currentTime <= totalTime)
        {
            currentTime += Time.deltaTime;
            float interpolationPoint = currentTime / totalTime;
            transform.localPosition = Vector3.Lerp(initialPosition, destinationPosition, interpolationPoint);
            yield return new WaitForEndOfFrame();
        }
        callBack();
    }

}

