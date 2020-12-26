using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static Action<Vector3> s_MouseKeyPressed;
    public static Action<Vector3> s_ScreenTouched;

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR

        if(Input.GetMouseButtonUp(0))
        {
            s_MouseKeyPressed?.Invoke(Input.mousePosition);
        }
#endif
        if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began && Input.touches[0].phase != TouchPhase.Began)
        {
            s_ScreenTouched?.Invoke(Input.touches[0].position);
        }

    }
}
