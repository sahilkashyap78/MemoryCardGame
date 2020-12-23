using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiResolution : MonoBehaviour
{
    private float m_PixelPerUnit = 100f;
    [SerializeField]
    private Camera m_MainCamera;
    [SerializeField] Vector2 m_RefrenceResolution = new Vector2(2048f, 1536f);
    
    void Start()
    {
        float targetRatio = Screen.width / m_RefrenceResolution.x;
        float targetWidth = Screen.height / 2;
        transform.localScale *= targetRatio; 
        m_MainCamera.orthographicSize = targetWidth / m_PixelPerUnit;
    }
}
