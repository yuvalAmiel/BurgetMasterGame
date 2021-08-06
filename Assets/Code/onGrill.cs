using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onGrill : MonoBehaviour
{
    public bool isCollideWithPlayer = false;
    public GameObject myPlayer;
    public joyButton m_getInfo;
    public Transform myFood;
    public bool m_onGrill = false;
    public List<Transform> myBurgers = new List<Transform>();

    public bool youCanClick = true;
    Animator TimeUpAnimatorLeft, TimeUpAnimatorRight;
    burgerStates myBurger;
    Animator playerAnimator;
    leftSideCollision m_LeftBurger;
    public bool leftBurgerOn = false, rightBurgerOn = false;

    void Start()
    {
        TimeUpAnimatorLeft = this.transform.GetChild(2).GetComponent<Animator>();
        TimeUpAnimatorRight = this.transform.GetChild(3).GetComponent<Animator>();
        playerAnimator = myPlayer.GetComponent<Animator>();
        m_LeftBurger = this.transform.GetChild(0).GetComponent<leftSideCollision>();
    }


    void Update()
    {
        if (myFood == null)
        {
            TimeUpAnimatorLeft.SetBool("Burned", false);
            TimeUpAnimatorLeft.SetBool("OldBurger", false);
            TimeUpAnimatorLeft.SetBool("TimeIsUp", false);

            TimeUpAnimatorRight.SetBool("Burned", false);
            TimeUpAnimatorRight.SetBool("OldBurger", false);
            TimeUpAnimatorRight.SetBool("TimeIsUp", false);
        }
        else
            CheckAnimationState();

        if (!youCanClick) 
        {
            if(m_getInfo.ReturnIfPressed())
                return;
            youCanClick = true;
        }
        if ((isCollideWithPlayer || m_LeftBurger.CollideWithPlayer) && m_getInfo.ReturnIfPressed() &&
            !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Running")
             && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("RunningWith")) // if theres a collision and the pickup button is pressed
        {
            youCanClick = false;
            if (m_onGrill)
            {
                if (myPlayer.transform.childCount <= 2)
                {
                    if(ValidateBurgerState())
                    {
                        myFood.transform.SetParent(myPlayer.transform);
                        FindObjectOfType<AudioManager>().Stop("BurgerOnGrill");
                        this.myFood = null;
                        m_onGrill = false;
                    }
                }
                return;
            }

            Transform myChild = myPlayer.transform.Find("Burger(Clone)").transform;
            if (myChild != null) // grill accepts only Burgers!!!
            {
                FindObjectOfType<AudioManager>().Play("BurgerOnGrill");

                myChild.transform.SetParent(this.transform); //parent is grill
                this.myFood = myChild;

                m_onGrill = true;
                //myFood.transform.localPosition = new Vector3(0f, 0.44f, 0.9f);
                if(isCollideWithPlayer)
                    myFood.transform.localPosition = new Vector3(0.8f, 0.65f, 0.8f);
                else
                    myFood.transform.localPosition = new Vector3(-0.8f, 0.65f, 0.8f);
                myBurger = this.myFood.GetComponent<burgerStates>();
                if(ValidateBurgerState())
                {
                    if(myBurger.GetState() == "Burned")
                        TimeUpAnimatorLeft.SetBool("Burned", true);
                    else
                        TimeUpAnimatorLeft.SetBool("OldBurger", true);
                }

                TimeUpAnimatorLeft.SetBool("TimeIsUp", true);
            }
        }
    }

    public void CheckAnimationState()
    {
        if (TimeUpAnimatorLeft.GetCurrentAnimatorStateInfo(0).IsName("RareState"))
            myBurger.UpdateBurgerState("Rare");
        else if (TimeUpAnimatorLeft.GetCurrentAnimatorStateInfo(0).IsName("MediumState"))
            myBurger.UpdateBurgerState("Medium");
        else if (TimeUpAnimatorLeft.GetCurrentAnimatorStateInfo(0).IsName("ReadyState")
            || TimeUpAnimatorLeft.GetCurrentAnimatorStateInfo(0).IsName("ReadyStateContinue"))
            myBurger.UpdateBurgerState("Ready");
        else if (TimeUpAnimatorLeft.GetCurrentAnimatorStateInfo(0).IsName("BurnedState"))
            myBurger.UpdateBurgerState("Burned");
    }

    bool ValidateBurgerState()
    {
        if(myBurger != null)
            return (myBurger.GetState() == "Ready" || myBurger.GetState() == "Burned");
        return false;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isCollideWithPlayer = true;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isCollideWithPlayer = false;
        }
    }
}
