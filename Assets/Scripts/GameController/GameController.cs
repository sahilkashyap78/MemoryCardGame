using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
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
    private const float CARDWIDTH = 2.333f;
    private const float CARDHEIGHT =3.166f;
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
    private int m_CardPairsCounter;
    private const float WAITING_TIME = 1f;
    private const float WONWAITING_TIME = 1f;
    [SerializeField]
    private Text m_TimerText;
    private float m_Timer;
    [SerializeField]
    private GameOverPopUp m_PopUp;
    private Card[] m_CardList;
    private const float MIN_TIME = 0f;
    private Text m_GameTimerText;
    private bool m_IsTextActive = true;
    private const string CAN_BLINK = "CanBlink";
    private Animator m_GameTimerAnimator;
    private int m_CountDownIndex;
    [SerializeField]
    private Text m_CountDownText;
    private int m_CardIdIndex;
    private Coroutine m_GameTimer = null;
    private bool m_BlinkTimer;
    private const int ZERO_INDEX = 0;
    private const int RESET_COUNTER = 0;
    private const string TOTALCOUNTDOWNSTRING = "3";
    private const string TOTALGAMETIMER = "30";
    private const float FLIPCARDWAIT = 0.5f;
    private const int TOTALCOUNTDOWN = 3;
    private const float GREEN_RANGE = 30f;
    private const float YELLOW_RANGE = 20f;
    private const float RED_RANGE = 10f;
    private const float BLINK_RANGE = 5f;
    private const float RESET_TIMER = 30F;
    private string[] m_CountDown = { "GO", "1", "2", "3" };
    [SerializeField]
    private GameObject m_CountDownBackground;
    //create array for the card list
    public enum State
    {
        INITIALIZE,
        PLAY,
        WIN,
        RESET,
        LOSE
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
        m_InputManager.s_CardTouched += GetClickedCard;
        m_NextCardCallBack += CheckNextCardStatus;
    }

    void OnDisable()
    {
        m_InputManager.s_CardTouched -= GetClickedCard;
        m_NextCardCallBack -= CheckNextCardStatus;
    }

    void Awake()
    {
        m_CardIdIndex = ZERO_INDEX;
        m_CountDownIndex = TOTALCOUNTDOWN;
        m_BlinkTimer = false;
        m_Timer = RESET_TIMER;
        m_GameTimerAnimator = m_TimerText.GetComponent<Animator>();
        m_GameTimerText = m_TimerText.GetComponent<Text>();
        m_CardPairsCounter = 0;
        m_TotalCards = m_Rows * m_Columns;
        m_CardList = new Card[m_TotalCards];
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


    IEnumerator GameTimer()
    {
        CheckInputStatus(m_CardIdIndex);
        while (m_Timer > MIN_TIME)
        {
            SetTimerColor();
            yield return new WaitForSeconds(WAITING_TIME);
            m_Timer--;
            m_TimerText.text = m_Timer.ToString();
        }

        m_State = State.LOSE;
        BlinkTimer();
        m_PopUp.ShowPopUp();        
    }

    void BlinkTimer()
    {
        m_BlinkTimer = !m_BlinkTimer;
        m_GameTimerAnimator.SetBool(CAN_BLINK, m_BlinkTimer);
    }


    private void SetTimerColor()  
    {
        if (m_Timer == YELLOW_RANGE)
        {
            m_GameTimerText.color = Color.yellow;
        }
        else if (m_Timer == RED_RANGE)
        {
            m_GameTimerText.color = Color.red;
        }
        else if(m_Timer == BLINK_RANGE)
        {
            BlinkTimer();
        }
    }

    void ResetTimer()
    {
        m_Timer = RESET_TIMER;
    }

    void GetClickedCard(Card card)
    {
        card.FlipCard();
        if (m_FirstCard == null)
        {
            m_FirstCard = card;
        }
        else
        {
            m_SecondCard = card;
            CheckCardType(m_FirstCard, m_SecondCard);
            
        }
    }

    IEnumerator Won()
    {
        if (m_BlinkTimer == true)
        {
            BlinkTimer();
        }
        yield return new WaitForSeconds(WONWAITING_TIME);
        if (ScoreManager.ScoreManagerInstance.Score < 0f)
        {
            m_State = State.LOSE;
        }
        else
        {
            m_State = State.WIN;
        }
        m_PopUp.ShowPopUp();
    }

    private void CheckCardType(Card firstCard, Card secondCard)
    {
        m_FirstCard = null;
        m_SecondCard = null;
        if (firstCard.CurrentType == secondCard.CurrentType)
        {
            m_CardPairsCounter++;
            ScoreManager.ScoreManagerInstance.IncrementScore();
            StartCoroutine(ScaleCards(firstCard, secondCard));
            if (m_CardPairsCounter >= m_TotalCards / 2)
            {
                StopCoroutine(m_GameTimer);
                StartCoroutine(Won());
            }
        }
        else
        {
            ScoreManager.ScoreManagerInstance.DecrementScore();
            StartCoroutine(UnFlipCards(firstCard, secondCard));
        }
    }

    IEnumerator ScaleCards(Card firstCard, Card secondCard)
    {
        yield return new WaitForSeconds(WAITING_TIME);
        firstCard.ScaleCard();
        secondCard.ScaleCard();
    }

    IEnumerator UnFlipCards(Card firstCard, Card secondCard)
    {
        yield return new WaitForSeconds(WAITING_TIME);
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
        for (int rows = 0; rows < m_Rows; rows++)
        {
            for (int columns = 0; columns < m_Columns; columns++)
            {
                m_CanPutNextCard = false;
                if(m_State == State.INITIALIZE)
                {
                    GameObject tempCard = Instantiate(m_CardPrefab, m_InitialPosition, Quaternion.identity, m_World);
                    Card card = tempCard.GetComponent<Card>();
                    m_CardList[m_CardIdIndex]= card;
                }
                m_CardList[m_CardIdIndex].Initialize(m_CardIdIndex, m_CardData[(int)m_FaceCardType[m_CardIdIndex]], m_InitialPosition, m_DestinationPosition, m_NextCardCallBack);
                while (m_CanPutNextCard == false)
                {
                    yield return new WaitForEndOfFrame();
                }
                m_DestinationPosition.x += CARDWIDTH + m_CardGap;
                m_CardIdIndex++;
            }
            m_DestinationPosition.x = m_World.position.x - (((float)m_Columns / 2 * (CARDWIDTH + m_CardGap)) - (CARDWIDTH /2f + m_CardGap / 2f));
            m_DestinationPosition.y -= CARDHEIGHT + m_CardGap;
        }
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        m_CountDownBackground.SetActive(true);
        m_CountDownText.gameObject.SetActive(true);
        while(m_CountDownIndex >= ZERO_INDEX)
        {
            
            m_CountDownText.text = m_CountDown[m_CountDownIndex];
            yield return new WaitForSeconds(WAITING_TIME);
            m_CountDownIndex--;
        }
        m_CountDownText.gameObject.SetActive(false);
        m_CountDownBackground.SetActive(false);
        m_GameTimer = StartCoroutine(GameTimer());
    }


    private void CheckInputStatus(int id)
    {
        if (id >= m_Rows * m_Columns)
        {
            m_State = State.PLAY;
        }
    }

    IEnumerator UnflipAllCards()
    {
        for (int index = 0; index < m_CardList.Length; index++)
        {
            if(!m_CardList[index].Canflip)//!
            {
                m_CardList[index].FlipCard();
                yield return new WaitForSeconds(FLIPCARDWAIT);
            }
            m_CardList[index].MoveToIdle();
        }
        StartCoroutine(MoveCardsToInitialPosition());
    }

    IEnumerator MoveCardsToInitialPosition()
    {
        for (int index = 0; index < m_CardList.Length; index++)
        {
            m_CanPutNextCard = false;
            StartCoroutine(m_CardList[index].MoveCard(m_CardList[index].transform.localPosition, m_InitialPosition, m_NextCardCallBack));
            while (m_CanPutNextCard == false)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        ShuffleCardType(m_FaceCardType);
        StartCoroutine(DistributeCard());
    }


    public void ResetGame()
    {
        ResetVariables();
        ScoreManager.ScoreManagerInstance.ResetScore();
        ResetTimer();
        StartCoroutine(UnflipAllCards());
    }

    private void ResetVariables()
    {
        m_State = State.RESET;
        m_GameTimerText.color = Color.green;
        m_CardIdIndex = ZERO_INDEX;
        m_CardPairsCounter = RESET_COUNTER;
        m_CountDownIndex = TOTALCOUNTDOWN;
        m_CountDownText.text = TOTALCOUNTDOWNSTRING;
        m_GameTimerText.text = TOTALGAMETIMER;
        m_DestinationPosition.x = m_World.position.x - (((float)m_Columns / 2 * (CARDWIDTH + m_CardGap)) - (CARDWIDTH / 2f + m_CardGap / 2f));
        m_DestinationPosition.y = m_World.position.y + (((float)m_Rows / 2 * (CARDHEIGHT + m_CardGap)) - (CARDHEIGHT / 2f + m_CardGap / 2f));
    }
}

