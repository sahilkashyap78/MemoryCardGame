using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

[Serializable]
public class CardData
{
    [SerializeField]
    private GameController.CardType m_CardType;
    [SerializeField]
    private Sprite m_FaceSprite;
    public GameController.CardType CardType
    {
        get
        {
            return m_CardType;
        }
    }
    public Sprite FaceSprite
    {
        get
        {
            return m_FaceSprite;
        }
    }
}


public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_CardPrefab;
    [SerializeField]
    private CardData[] m_CardData; 
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
    private const float MIN_CARDS = 8f;
    private Action m_NextCardCallBack;
    private bool m_CanPutNextCard;
    [SerializeField]
    private Sprite[] m_FaceCards;
    private int m_FaceSprirteIndex;
    private List<CardType> m_FaceCardType = new List<CardType>();
    private List<int> m_RandomList = new List<int>();
    private int m_TotalCards;
    private Card m_FirstCard;
    private Card m_SecondCard;
    private int m_CardAssigningCounter;
    private int m_CardPairsCounter;

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
        m_InputManager.s_CardTouched += InitializeCards;
        m_NextCardCallBack += CheckNextCardStatus;
    }

    void OnDisable()
    {
        m_InputManager.s_CardTouched -= InitializeCards;
        m_NextCardCallBack -= CheckNextCardStatus;
    }

    void Awake()
    {
        m_CardAssigningCounter = 0;
        m_CardPairsCounter = 0;
        m_TotalCards = m_Rows * m_Columns;
        m_State = State.INITIALIZE;
        m_DestinationPosition.x = m_World.position.x - (((float)m_Columns / 2 * (CARDWIDTH + m_CardGap)) - (CARDWIDTH / 2f + m_CardGap / 2f));
        m_DestinationPosition.y = m_World.position.y + (((float)m_Rows / 2 * (CARDHEIGHT + m_CardGap)) - (CARDHEIGHT / 2f + m_CardGap / 2f));
        m_InitialPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height * -0.6f, 0f));
        SetRandomList();
        ShuffleList(m_RandomList);
        SetFaceId();
        ShuffleCardType(m_FaceCardType);
    }

    void Start()
    {
        if(m_TotalCards >= MIN_CARDS)
        {
            StartCoroutine(DistributeCard());
        }
    }

   void InitializeCards(Card card)
    {
        if(card.Canflip)
        {
            m_CardAssigningCounter++;
            if(m_CardAssigningCounter == 1 && m_FirstCard == null)
            {
                m_FirstCard = card;
                m_FirstCard.FlipCard();
            }
            else if(m_CardAssigningCounter == 2 && m_SecondCard == null)
            {
                m_SecondCard = card;
                m_SecondCard.FlipCard();
                Card firstCard = m_FirstCard;
                Card secondCard = m_SecondCard;
                m_FirstCard = null;
                m_SecondCard = null;
                m_CardAssigningCounter = 0;
                if (firstCard.CurrentType == secondCard.CurrentType)
                {
                    if(m_CardPairsCounter >= m_TotalCards/2)
                    {
                        Debug.Log("You win");
                    }
                    else
                    {
                        m_CardPairsCounter++;
                    }
                }
                else
                {
                    StartCoroutine(UnFlipCards(firstCard, secondCard));
                }
            }
        }
        //card.FlipCard();
    }

    IEnumerator UnFlipCards(Card firstCard, Card secondCard)
    {
        yield return new WaitForSeconds(1f);
        firstCard.FlipCard();
        secondCard.FlipCard();
    }

    private void SetRandomList()
    {
        for(int index = 0; index < m_FaceCards.Length; index++)
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
        int randomizePoint = (cardPairs / m_FaceCards.Length) * m_FaceCards.Length; 
        for (int index = 0; index < (m_Rows * m_Columns) / 2; index++)
        {
            if ((faceSprirteIndex >= m_FaceCards.Length) || takeRandomId)
            {
                faceSprirteIndex = 0;
                if (index + 1 >= randomizePoint)
                {
                    takeRandomId = true;
                    faceSprirteIndex = m_RandomList[randomizeIndex];
                    randomizeIndex++;
                }
            }
            m_FaceCardType.Add((CardType)faceSprirteIndex);
            m_FaceCardType.Add((CardType)faceSprirteIndex);
            faceSprirteIndex++;
        }
    }

    void ShuffleCardType(List<CardType> faceCardIds)
    {
        for (int index = 0; index < faceCardIds.Count; index++)
        {
            int tempId = (int)faceCardIds[index];
            int randomId = Random.Range(index, faceCardIds.Count);
            faceCardIds[index] = faceCardIds[randomId];
            faceCardIds[randomId] = (CardType)tempId;
        }
    }

    void ShuffleList(List<int> faceCardIds)
    {
        for (int index = 0; index < faceCardIds.Count; index++)
        {
            int tempId = faceCardIds[index];
            int randomId = Random.Range(index, faceCardIds.Count);
            faceCardIds[index] = faceCardIds[randomId];
            faceCardIds[randomId] = tempId;
        }
    }

    void CheckNextCardStatus()
    {
        m_CanPutNextCard = true;
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
                card.Initialize(tempCardId, m_CardData[(int)m_FaceCardType[tempCardId]], m_InitialPosition, m_DestinationPosition, m_NextCardCallBack);
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


