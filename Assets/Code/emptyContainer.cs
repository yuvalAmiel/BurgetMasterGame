using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emptyContainer : MonoBehaviour
{
    TutorialManager m_Tutorial;
    public bool m_takeFrom = false;
    public bool isCollideWithPlayer = false;
    public Transform myFood;
    public GameObject myPlayer;
    public joyButton m_getInfo; // need to check how
    Color m_Color;
    PlayerCollisions m_UpdateCollision;
    public bool youCanClick = true;
    public bool isCuttingBoardOn = false; // public for debug
    Animator playerAnimator;
    public float TopBunDistance = 0.3f;
    private float burgerInBun = 0.2f;
    public bool ForTutorial = false;
    void Start()
    {
        m_Color = this.gameObject.GetComponent<Renderer>().materials[1].color;
        m_UpdateCollision = myPlayer.GetComponent<PlayerCollisions>();
        playerAnimator = myPlayer.GetComponent<Animator>();
        m_Tutorial = FindObjectOfType<TutorialManager>();
    }


    // Update is called once per frame
    void Update()
    {
        if (m_Tutorial.InTutorial() && !ForTutorial)
            return;
        if (!youCanClick)
        {
            if (m_getInfo.ReturnIfPressed())
                return;
            youCanClick = true;
        }

        if (isCollideWithPlayer && m_getInfo.ReturnIfPressed() &&
           !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Running")
             && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("RunningWith")) // if theres a collision and the pickup button is pressed
        {
            youCanClick = false;
            if (m_takeFrom)
            {
                if (myPlayer.transform.childCount <= 2)
                    PlayerTakesFood();

                else // object inside another object
                    FoodInsideFood(myPlayer.transform.GetChild(2));
            }

            else if (myPlayer.transform.childCount == 3) // object on container
                HandleFoodOnContainer(myPlayer.transform.GetChild(2));
        }
    }

    void FoodInsideFood(Transform playerChild)
    {
        if (BreadOnBread(playerChild))
            return;

        if (BurgerOnBurger(playerChild))
            return;


        if (playerChild.tag == "Food" || playerChild.tag == "Sliced") // anything but plate
        {
            Transform myBread = myFood.transform.Find("Bread(Clone)"); // if my plate contains bread
            Transform myTopBun = null, myBottomBun = null;
            if (!myBread && myFood.name == "Bread(Clone)") //if my food is already bread
                myBread = myFood;

            if (myBread != null && playerChild.name != "Bread(Clone)") // dont wanna put bread on bread
            {
                myTopBun = myBread.transform.Find("TopBun");
                myBottomBun = myBread.transform.Find("BottomBun");

                myTopBun.transform.localPosition = new Vector3(
                      myTopBun.transform.localPosition.x,
                      myTopBun.transform.localPosition.y + TopBunDistance, // move up a little
                      myTopBun.transform.localPosition.z);

            }
            if (myBread != null)
                playerChild.transform.SetParent(myBread.transform);
            else
                playerChild.transform.SetParent(myFood.transform);

            if (playerChild.name != "Bread(Clone)") // not bread
            {
                if (myTopBun != null)
                {
                    playerChild.transform.localPosition = new Vector3(
                        0f,
                        myBottomBun.transform.localPosition.y + SumBurgers(myBread), // move down a little
                        0f);
                }
                else if (playerChild.name == "Burger(Clone)")
                    playerChild.transform.localPosition =
                         playerChild.GetComponent<FoodLocation>().foodPositionOnPlate;
                else
                    playerChild.transform.localPosition =
                        playerChild.GetComponent<FoodLocation>().foodPositionOnContainer;
            }
            else
            {
                playerChild.transform.localPosition =
                    playerChild.GetComponent<FoodLocation>().foodPositionOnPlate;
            }
        }
    }

    bool BurgerOnBurger(Transform playerChild)
    {
        if(playerChild.name == "Burger(Clone)")
        {
            if(myFood.name == "Burger(Clone)")
                return true;
            if (myFood.Find("Bread(Clone") == null && myFood.Find("Burger(Clone)") != null)
                return true;
            if (myFood.tag == "Plate" && myFood.childCount == 0)
                return true;
        }
        if (myFood.tag == "Plate" && myFood.childCount == 0 && playerChild.tag == "Sliced")
            return true;
        return false;
    }

    bool BreadOnBread(Transform playerChild)
    {
        if (playerChild.name == "Bread(Clone)")
        {
            if(myFood.name == "Bread(Clone)" || myFood.Find("Bread(Clone)"))
                return true;
            if (myFood.name == "Burger(Clone)")
                return true;
            if (myFood.tag == "Plate" && myFood.Find("Burger(Clone)") != null)
                return true;
        }

        return false;
    }

    float SumBurgers(Transform Bread)
    {
        int found = 0;
        float distance = 0f;
        Debug.Log("the transform is " + Bread.name);

        for(int i = 0; i < Bread.childCount; i++)
        {
            Transform Burger = Bread.GetChild(i);
            if (Burger.name == "Burger(Clone)" || Burger.tag == "Sliced")
            {
                distance += 0.3f;
                found++;
            }
        }
        Debug.Log("found is " + found);
        Debug.Log("distance is " + distance);
        if (found <= 1)
            return 0.2f;
        distance -= 0.1f;
        return distance;
    }

    void PlayerTakesFood()
    {
        myFood.transform.SetParent(myPlayer.transform);
        playerAnimator.SetBool("IsHolding", true);
        m_takeFrom = false;
        this.myFood = null;
    }

    void HandleFoodOnContainer(Transform playerChild)
    {
        if (playerChild.tag == "Plate")
            FindObjectOfType<AudioManager>().Play("PlacePlate");

        playerChild.transform.SetParent(this.transform);
        this.myFood = playerChild;

        myFood.transform.localPosition = myFood.GetComponent<FoodLocation>().foodPositionOnContainer;
        m_takeFrom = true;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && !m_UpdateCollision.getInCollision())
        {
            m_UpdateCollision.setInCollision(true);
            isCollideWithPlayer = true;
            //this.gameObject.GetComponent<Renderer>().material.color = Color.HSVToRGB(0f, 0f ,0.9f);
            //this.gameObject.GetComponent<Renderer>().materials[1].color = Color.HSVToRGB(0.078f, 0.62f, 0.8f);
            // this.gameObject.GetComponent<Renderer>().material.color = Color.HSVToRGB(0.07f, 0.584f, 0.71f);
            this.gameObject.GetComponent<Renderer>().materials[1].color = Color.HSVToRGB(0.042f, 0.67f, 0.5f);
        }
    }

    void OnCollisionExit(Collision other)
    {
        //if (other.gameObject.CompareTag("Player"))
       // {
            m_UpdateCollision.setInCollision(false);
            isCollideWithPlayer = false;
            //this.gameObject.GetComponent<Renderer>().material.color = m_Color;
            this.gameObject.GetComponent<Renderer>().materials[1].color = m_Color;
        //}
    }


}
