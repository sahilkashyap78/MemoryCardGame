using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private InputManager m_InputManager;
    [SerializeField]
    private Card m_Card;

    void OnEnable()
    {
#if UNITY_EDITOR
        InputManager.s_MouseKeyPressed += MouseKeyPressed;
#endif
        InputManager.s_ScreenTouched += OnScreenTouched;
    }

    void OnDisable()
    {
#if UNITY_EDITOR
        InputManager.s_MouseKeyPressed -= MouseKeyPressed;
#endif
        InputManager.s_ScreenTouched -= OnScreenTouched;
    }

    void MouseKeyPressed(Vector3 position)
    {
        m_Card.CheckTouch(position);
    }

    void OnScreenTouched(Vector3 position)
    {
        m_Card.CheckTouch(position);
    }

       
}
