using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCaster : MonoBehaviour
{
    [SerializeField]
    private Camera m_MainCamera;
    private const int CARD_LAYER = 1 << 8;
    private const float MIN_RAYCASTDISTANCE = 2f;
    private const float ZERO_POSITION = 0f;
    

    public GameObject RaycastGameObject(Vector3 position)
    {
        float x_Position = m_MainCamera.ScreenToWorldPoint(new Vector3(position.x, ZERO_POSITION, ZERO_POSITION)).x;
        float y_Position = m_MainCamera.ScreenToWorldPoint(new Vector3(ZERO_POSITION, position.y, ZERO_POSITION)).y;
        transform.position = new Vector3(x_Position, y_Position, ZERO_POSITION);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward, MIN_RAYCASTDISTANCE, CARD_LAYER);
        if (hit.collider != null)
        {
            
            return hit.collider.gameObject;
        }
        return null;
    }
}
