using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField]
    private Transform m_Raycaster;
    const int CARD_LAYER = 1 << 8;

    void Update()
    {
       
    }

    public  void CheckTouch(Vector3 position)
    {
        DetectCard(position);
    }

    private void DetectCard(Vector3 position)
    {
        float x_Axis = Camera.main.ViewportToWorldPoint(new Vector3(position.x / Screen.width, 0f, 0f)).x;
        float y_Axis = Camera.main.ViewportToWorldPoint(new Vector3(0f, position.y / Screen.height, 0f)).y;
        m_Raycaster.position = new Vector3(x_Axis, y_Axis, 0f);   //Camera.main.ViewportToWorldPoint(Input.mousePosition);
        if (Physics2D.Raycast(m_Raycaster.position, m_Raycaster.forward, 2f, CARD_LAYER))
        {
            Debug.Log("CARD TOUCED");
        }
    }
}
