using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private RayCaster m_RayCaster;
    void Awake()
    {
        Input.multiTouchEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR

        if(Input.GetMouseButtonUp(0))
        {
            GameObject tempCardObject = m_RayCaster.GetScreenPosition(Input.mousePosition);

            if (tempCardObject != null)
            {
                Card cardObject = tempCardObject.GetComponent<Card>();
                cardObject.DetectCard();
            }

        }
#endif
        if(Input.touchCount > 0)
        {
           Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began )
            {
                Vector3 tempPosition = new Vector3(touch.position.x, touch.position.y, 0f);
                GameObject tempCardObject = m_RayCaster.GetScreenPosition(tempPosition);
                if (tempCardObject != null)
                {
                    Card cardObject = tempCardObject.GetComponent<Card>();
                    cardObject.DetectCard();
                }
            }
        }
    }
}



/*if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 tempPosition = new Vector3(touch.position.x, touch.position.y , 0f);
            s_ScreenTouched?.Invoke(tempPosition);
        }*/
