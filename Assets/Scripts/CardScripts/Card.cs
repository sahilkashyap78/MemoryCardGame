using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField]
    private Transform m_Raycaster;
    private Sprite m_CardType;

    public void DetectCard()
    {
        Debug.Log("Card Detected");
    }
}
