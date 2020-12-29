using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private RayCaster m_RayCaster;
    private Card m_TouchDownCard = null;
    private Card m_TouchUpCard = null;
    [SerializeField]
    private Text m_Text;
    private int m_TouchCount;

    void Awake()
    {
        Input.multiTouchEnabled = false;
        m_TouchCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR

        if(Input.GetMouseButtonDown(0))
        {
            GameObject tempCardObject = m_RayCaster.RaycastGameObject(Input.mousePosition);
            if (tempCardObject != null)
            {
                m_TouchDownCard = tempCardObject.GetComponent<Card>();
            }
            else
            {
                m_TouchDownCard = null;
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            GameObject tempCardObject = m_RayCaster.RaycastGameObject(Input.mousePosition);
            if (tempCardObject != null)
            {
                m_TouchUpCard = tempCardObject.GetComponent<Card>();

                if(m_TouchUpCard != null && m_TouchDownCard != null)
                {
                    if (m_TouchDownCard.id == m_TouchUpCard.id)
                    {
                        m_TouchCount++;
                        m_Text.text = "Touch"+" "+m_TouchCount;
                    }
                }
            }
            else
            {
                m_TouchUpCard = null;
            }
        }

#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
           Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                Vector3 tempPosition = new Vector3(touch.position.x, touch.position.y, 0f);
                GameObject tempCardObject = m_RayCaster.RaycastGameObject(tempPosition);
                if (tempCardObject != null)
                {
                    m_TouchDownCard = tempCardObject.GetComponent<Card>();
                }
                else
                {
                    m_TouchDownCard = null;
                }
            }

            if(touch.phase == TouchPhase.Ended)
            {
                Vector3 tempPosition = new Vector3(touch.position.x, touch.position.y, 0f);
                GameObject tempCardObject = m_RayCaster.RaycastGameObject(tempPosition);
                if (tempCardObject != null)
                {
                    m_TouchUpCard = tempCardObject.GetComponent<Card>();

                    if (m_TouchUpCard != null && m_TouchDownCard != null)
                    {
                        if (m_TouchDownCard.id == m_TouchUpCard.id)
                        {
                           m_TouchCount++;
                           m_Text.text = "Touch"+" "+m_TouchCount;
                        }
                        m_TouchDownCard = null;
                        m_TouchUpCard = null;
                    }
                }
                else
                {
                    m_TouchUpCard = null;
                }
            }

        }
#endif
    }
}



/*
                Vector3 tempPosition = new Vector3(touch.position.x, touch.position.y, 0f);
                GameObject tempCardObject = m_RayCaster.RaycastGameObject(tempPosition);
                if (tempCardObject != null)
                {
                    Card cardObject = tempCardObject.GetComponent<Card>();
                    Debug.Log(cardObject.id);
                    cardObject.DetectCard();
                }*/
