using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_CardPrefab;
    [SerializeField]
    private int m_Rows;
    [SerializeField]
    private int m_Columns;
    [SerializeField]
    private Transform m_World;
    [SerializeField]
    InputManager m_InputManager;
    private float m_CardGap = 1f;
    private Vector3 m_DestinationPosition;
    Vector3 m_InitialPosition;
    private const float CARDWIDTH = 2.8f;
    private const float CARDHEIGHT =3.8f;
    private Action m_NextCardCallBack;
    private bool m_CanPutNextCard;
    [SerializeField]
    private List<Sprite> m_FaceSprites = new List<Sprite>();
    private int faceSprirteIndex;
    private List<int> m_FaceSpriteIds = new List<int>();
    private List<int> m_RandomList = new List<int>();
    private int m_TotalCards;

    public enum State
    {
        INITIALIZE,
        PLAY
    }

    public enum CardType
    {
        KINGOFDIAMOND,
        ACEOFHEART,
        ACEOFSPADE,
        KINGOFSPADE
    };

    private State m_State;
    public State currentState
    {
        get
        {
            return m_State;
        }
    }

    void OnEnable()
    {
        m_NextCardCallBack += CheckNextCardStatus;
    }

    void OnDisable()
    {
        m_NextCardCallBack -= CheckNextCardStatus;
    }

    void Awake()
    {
        m_TotalCards = m_Rows * m_Columns;
        m_State = State.INITIALIZE;
        m_DestinationPosition.x = m_World.position.x - (((float)m_Columns / 2 * (CARDWIDTH + m_CardGap)) - (CARDWIDTH / 2f + m_CardGap / 2f));
        m_DestinationPosition.y = m_World.position.y + (((float)m_Rows / 2 * (CARDHEIGHT + m_CardGap)) - (CARDHEIGHT / 2f + m_CardGap / 2f));
        m_InitialPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height * -0.6f, 0f));
        SetRandomList();
        ShuffleList(m_RandomList);
        SetFaceId();
        ShuffleList(m_FaceSpriteIds);
    }

    private void SetRandomList()
    {
        for(int index = 0; index < m_FaceSprites.Count; index++)
        {
            m_RandomList.Add(index);
        }
    }

    private void SetFaceId()
    {
        bool takeRandomId = false;
        int faceSprirteIndex = 0;
        int randomizeIndex = 0;
        int cardPairs = m_TotalCards / 2;
        int randomizePoint = (cardPairs / m_FaceSprites.Count) * m_FaceSprites.Count; 
       // Debug.Log(randomizePoint);
        for (int index = 0; index < (m_Rows * m_Columns) / 2; index++)
        {
            if ((faceSprirteIndex >= m_FaceSprites.Count) || takeRandomId)
            {
                faceSprirteIndex = 0;
                if (index + 1 >= randomizePoint)
                {
                    takeRandomId = true;
                    faceSprirteIndex = m_RandomList[randomizeIndex];
                    randomizeIndex++;
                }
            }
            m_FaceSpriteIds.Add(faceSprirteIndex);
            m_FaceSpriteIds.Add(faceSprirteIndex);
            faceSprirteIndex++;
        }
    }

    void ShuffleList(List<int> faceCardIds)
    {
        for (int index = 0; index < faceCardIds.Count; index++)
        {
            int tempId = faceCardIds[index];
            int randomId = Random.Range(tempId, faceCardIds.Count);
            faceCardIds[index] = faceCardIds[randomId];
            faceCardIds[randomId] = tempId;
        }
    }

    void Start()
    {
        //StartCoroutine(CheckNumbers());
        StartCoroutine(DistributeCard());
    }

    void CheckNextCardStatus()
    {
        m_CanPutNextCard = true;
    }

    IEnumerator CheckNumbers()
    {
        for(int index =0; index< m_FaceSpriteIds.Count; index++)
        {
            Debug.Log(m_FaceSpriteIds[index]);
            yield return null;
        }
    }
    IEnumerator DistributeCard()
    {
        int tempCardId = 0;
        for (int rows = 0; rows < m_Rows; rows++)
        {
            for (int columns = 0; columns < m_Columns; columns++)
            {
                m_CanPutNextCard = false;
                GameObject tempCard = Instantiate(m_CardPrefab, m_InitialPosition, Quaternion.identity, m_World);
                Card card = tempCard.GetComponent<Card>();
                card.Initialize(tempCardId, (CardType)m_FaceSpriteIds[tempCardId], m_InitialPosition, m_DestinationPosition, m_NextCardCallBack);
                while (m_CanPutNextCard == false)
                {
                    yield return new WaitForEndOfFrame();
                }
                m_DestinationPosition.x += CARDWIDTH + m_CardGap;
                tempCardId++;
            }
            m_DestinationPosition.x = m_World.position.x - (((float)m_Columns / 2 * (CARDWIDTH + m_CardGap)) - (CARDWIDTH /2f + m_CardGap / 2f));
            m_DestinationPosition.y -= CARDHEIGHT + m_CardGap;
        }
        CheckInputStatus(tempCardId);
    }

    private void CheckInputStatus(int id)
    {
        if (id >= m_Rows * m_Columns)
        {
            m_State = State.PLAY;
        }
    }
}


