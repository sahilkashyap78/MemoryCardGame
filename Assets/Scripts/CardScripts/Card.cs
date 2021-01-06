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

    public float id
    {
        get
        {
            return m_Id;
        }
    }

    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(int id, Vector3 initialPoaition, Vector3 destinationposition, Action nextCardCallBack)
    {
        m_Id = id;
        m_InitialPosition = initialPoaition;
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

