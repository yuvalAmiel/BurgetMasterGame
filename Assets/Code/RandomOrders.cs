using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class RandomOrders : MonoBehaviour
{
    public GameObject[] Orders;

    public Transform myCanvas;
    int randomInt, m_CurrentOrder = -1;

    RectTransform m_RectTransform;
    checkerScript m_RecievedDish;

    public List<GameObject> myOrders = new List<GameObject>();
    public List<int> m_MyOrdersLocation = new List<int>();
    public List<float> m_MyOrdersTime = new List<float>();
    int XPos = 20, YPos = -20;
    private bool FirstOrderCompleted = false, InCreation = false;

    public CoinCollection m_CoinCollection;
    private float outOfTime = 0;
    private int RandomLevel = 4000;

    public ProgressBox m_TempBox;
    TutorialManager m_Tutorial;

    void Start()
    {
        m_Tutorial = FindObjectOfType<TutorialManager>();
        FirstOrderCompleted = false;
        m_RecievedDish = GetComponent<checkerScript>();
        CreateNewOrder(true);
    }

    void CoolDeadOrder()
    {
        myOrders[0].transform.DOMove(new Vector3(-200f, 380f, 0f), 2f).SetEase(Ease.InOutBack).OnComplete(() => {
            //executes whenever coin reach target position
            Debug.Log("cmplaetea");
            DestroyDish(true, 0);
        });
    }

    void CreateNewOrder(bool firstTime)
    {
        if (!firstTime)
            randomInt = Random.Range(0, Orders.Length);
        else
        {
            randomInt = 0;
        }

        myOrders.Add(Instantiate(Orders[randomInt]) as GameObject);
        m_MyOrdersTime.Add(Time.realtimeSinceStartup);
        m_CurrentOrder++;
        m_RectTransform = myOrders[m_CurrentOrder].GetComponent<RectTransform>(); // for image
        FindObjectOfType<AudioManager>().Play("NewOrder");
        myOrders[m_CurrentOrder].transform.SetParent(myCanvas.transform);
        myOrders[m_CurrentOrder].transform.SetAsFirstSibling(); // behind everything on canvas


        handleOnjectSizeNLocation(m_RectTransform);
    }

    void handleOnjectSizeNLocation(RectTransform m_RectTransform)
    {
        m_RectTransform.anchoredPosition = new Vector2(XPos + (160 * m_CurrentOrder), YPos);
        m_MyOrdersLocation.Add(XPos + (220 * m_CurrentOrder));

        m_RectTransform.anchorMin = new Vector2(0, 1);
        m_RectTransform.anchorMax = new Vector2(0, 1);
        m_RectTransform.pivot = new Vector2(0, 1);

        m_RectTransform.localScale = new Vector3(2, 2, 2);
    }


    void Update()
    {

        if (m_RecievedDish.myFood != null) // there is food on checker
        {
            if (m_CurrentOrder >= 0)
                CompareThem();
            else
                DestroyDish(false, -1);
        }
        else
            checkForPressedOrders();

        if (!m_Tutorial.InTutorial())
        {

            outOfTime = 0;
            for (int i = 0; i < myOrders.Count; i++)
            {
                if (Time.time - m_MyOrdersTime[i] > myOrders[i].GetComponent<OrderHandler>().orderTime - 15)
                {
                    outOfTime++;
                    if (!FindObjectOfType<AudioManager>().IsPlaying("OrderTimeOut"))
                    {
                        FindObjectOfType<AudioManager>().Play("OrderTimeOut");


                    }
                    Debug.Log("in 10 seconds condition");
                    if (myOrders[i].GetComponent<OrderHandler>().GetTimeAnimationState())
                    {
                        DestroyDish(true, i);
                        m_TempBox.UpdateProgress(-0.1f);
                    }
                }

            }
            if (outOfTime == 0)
                FindObjectOfType<AudioManager>().Stop("OrderTimeOut");
        }

        if (Time.timeScale == 1) // the game isn't paused
            checkForOrdersToCreate();
    }

    void checkForOrdersToCreate()
    {
        if (FirstOrderCompleted || myOrders.Count == 0)
        {
            if (FirstOrderCompleted)
            {
                Orders[0].GetComponent<OrderHandler>().ChangeTime(60);
            }
            if (myOrders.Count == 0 && !InCreation)
            {
                StartCoroutine(WaitBeforeCreation());
            }

            else if (myOrders.Count < 4)
            {
                randomInt = Random.Range(0, RandomLevel);
                if (randomInt % (RandomLevel - 1) == 0)
                {
                    CreateNewOrder(false);
                }
            }
        }
    }
    IEnumerator WaitBeforeCreation()
    {
        InCreation = true;
        yield return new WaitForSecondsRealtime(4);


        CreateNewOrder(false);
        InCreation = false;
    }

    void checkForPressedOrders()
    {
        int indexOfActive = -1;
        float maxTime = 0, timeOfActive;
        for(int i = 0; i < myOrders.Count; i++)
        {
            timeOfActive = myOrders[i].transform.GetComponent<OrderHandler>().getActiveTime();
            if (maxTime < timeOfActive)
            {
                maxTime = timeOfActive;
                indexOfActive = i;
            }
        }
        if(indexOfActive != -1) // there is an active order
        {
            for (int i = 0; i < myOrders.Count; i++) // going threw all orders
            {
                if (myOrders[i] != myOrders[indexOfActive])
                    myOrders[i].transform.GetChild(0).gameObject.SetActive(false);
                MoveToTheLeft();
            }

            MoveToTheRight(indexOfActive); // change position of the following orders
            return;
        }

        MoveToTheLeft();


    }

    void MoveToTheLeft()
    {
        for (int i = 0 ; i < myOrders.Count; i++) // change position back
        {
            m_RectTransform = myOrders[i].GetComponent<RectTransform>();
            m_RectTransform.anchoredPosition = new Vector2(m_MyOrdersLocation[i], YPos);
        }
    }

    void MoveToTheRight(int i)
    {
        for(int order = i + 1; order < myOrders.Count; order++)
        {
            m_RectTransform = myOrders[order].GetComponent<RectTransform>(); // for image

            if (m_MyOrdersLocation[order] != m_RectTransform.anchoredPosition.x)
                break;
            m_RectTransform.anchoredPosition = new Vector2(m_RectTransform.anchoredPosition.x + 160, YPos);
           
        }
    }

    bool findAMatch(Transform order)
    {
        Transform dishObjects = m_RecievedDish.myFood.transform; //the dish that is on checker
        int NumOfObjects = 1; // how many food is in bread (1 for the bread itself)

        dishObjects = dishObjects.Find("Bread(Clone)");
        if (dishObjects == null)
            return false;

        for(int i = 0; i <dishObjects.childCount; i++)
        {
            if(dishObjects.GetChild(i).tag == "Food")
               NumOfObjects++;
        }
        if (NumOfObjects != order.childCount)
            return false;

        for (int OrderIngradient = 0; OrderIngradient < order.childCount; OrderIngradient++)
        {
            string dishName = order.GetChild(OrderIngradient).name; // the name in variable
            if (dishName == "Bread")
                continue;
            Debug.Log(dishName);
            if (dishObjects.transform.Find(dishName + "(Clone)") == null) //null so doesn't exists
                return false;
            if (dishObjects.transform.Find(dishName + "(Clone)") != null && (dishName + "(Clone)" == "Burger(Clone)"))
            {
                if(!ValidateBurgerState(dishObjects.transform.Find(dishName + "(Clone)")))
                    return false;
            }
        }
        return true;
    }

    bool ValidateBurgerState(Transform Burger)
    {
        burgerStates tempBurger = Burger.GetComponent<burgerStates>();
        return tempBurger.GetState() == "Ready";
    }

    void CompareThem()
    {
        if (m_RecievedDish.myFood.tag == "Plate") // it's a plate
        {
            for (int i = 0; i <= m_CurrentOrder; i++)
            {
                if (findAMatch(myOrders[i].transform.GetChild(0)))
                {
                    m_RecievedDish.setGreenColor();
                    FindObjectOfType<AudioManager>().Play("CorrectOrder");

                    m_CoinCollection.StartCoinMove
                        (m_RecievedDish.myFood.transform.position,
                       myOrders[i].transform.GetComponent<OrderHandler>().getOrderValue());
                      m_TempBox.UpdateProgress
                          (myOrders[i].transform.GetComponent<OrderHandler>().getProgressValue());
                    // m_TempBox.UpdateProgress(0.5f);
                    FirstOrderCompleted = true;
                    DestroyDish(true, i);

                    if (m_TempBox.GetSliderValue() >= 0.5)
                        RandomLevel = 3000;
                    return;
                }
                else
                {
                    FindObjectOfType<AudioManager>().Play("WrongOrder");
                    m_RecievedDish.setRedColor();
                    m_TempBox.UpdateProgress
                        (-0.1f); 
                }
            }
        }
        
        DestroyDish(false, -1);
    }

    void DestroyDish(bool WithOrder, int deleteOrder)
    {
        if(m_RecievedDish.myFood != null)
            Destroy(m_RecievedDish.myFood.gameObject, 0.3f);
        m_RecievedDish.myFood = null;


        if (WithOrder)
        {
            Destroy(myOrders[deleteOrder].gameObject, 0.3f);
            myOrders.Remove(myOrders[deleteOrder]); // remove gameobject
            moveBackOtherOrders(deleteOrder);
            m_MyOrdersLocation.Remove(m_MyOrdersLocation[deleteOrder]);
            m_MyOrdersTime.Remove(m_MyOrdersTime[deleteOrder]);
            m_CurrentOrder--;
        }
    }

    void moveBackOtherOrders(int endHere)
    {
        for (int i = m_MyOrdersLocation.Count - 1; i > endHere; i--)
            m_MyOrdersLocation[i] = m_MyOrdersLocation[i - 1];
    }
}
