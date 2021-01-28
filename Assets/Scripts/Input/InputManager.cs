using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InputManager : MonoBehaviour
{
    public Action<Card> s_CardTouched;
    [SerializeField]
    private RayCaster m_RayCaster;
    private Card m_TouchDownCard = null;
    private Card m_TouchUpCard = null;
    private int m_TouchCount;
    private const int LEFT_CLICK = 0;
    [HideInInspector]
    public bool m_CanTouchCard;
    [SerializeField]
    GameController m_GameController;
    

    void Awake()
    {
        Input.multiTouchEnabled = false;
        m_TouchCount = 0;
        m_CanTouchCard = false;
    }

    void Update()
    {
        Touch currentTouch;
#if UNITY_EDITOR
        currentTouch = new Touch();
        currentTouch.position = Input.mousePosition;
        currentTouch.phase = TouchPhase.Stationary;
        if (Input.GetMouseButtonDown(LEFT_CLICK))
        {
            currentTouch.phase = TouchPhase.Began;
        }
        else if (Input.GetMouseButtonUp(LEFT_CLICK))
        {
            currentTouch.phase = TouchPhase.Ended;
        }


#elif UNITY_ANDROID
        currentTouch = new Touch();
        if(Input.touchCount > 0)
        {
            currentTouch = Input.GetTouch(0);
        }

#endif
        CheckCurrentPhase(currentTouch);

    }

    private void CheckCurrentPhase(Touch currentTouch)
    {
        if(m_GameController.currentState == GameController.State.PLAY)
        {
            if (currentTouch.phase == TouchPhase.Began)
            {
                m_TouchDownCard = GetCardReference(currentTouch);
            }
            else if (currentTouch.phase == TouchPhase.Ended)
            {
                m_TouchUpCard = GetCardReference(currentTouch);
                CompareCards();
            }
        }
    }

    private Card GetCardReference(Touch currentTouch)
    {
        GameObject raycastedObject = m_RayCaster.RaycastGameObject(currentTouch.position);
        if (raycastedObject != null)
        {
            Card tempCard = raycastedObject.GetComponent<Card>();
            return tempCard;
        }
        return null;
    }

    private void CompareCards()
    {
        if (m_TouchUpCard != null && m_TouchDownCard != null)
        {
            if (m_TouchDownCard.id == m_TouchUpCard.id && m_TouchDownCard.Canflip == true)
            {
                s_CardTouched?.Invoke(m_TouchDownCard);
            }
        }
    }
}
