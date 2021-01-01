using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField]
    private GameController m_GameController;
    [SerializeField]
    private Sprite m_CardType;
    private SpriteRenderer m_SpriteRenderer;
    [HideInInspector]
    public int id;

    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        if(m_GameController.tempCardId < m_GameController.TotalCards)
        {
            id = m_GameController.tempCardId;
            m_GameController.tempCardId++;
        }
    } 

    void OnDisable()
    {
        m_GameController.tempCardId = 0;
    }
}
