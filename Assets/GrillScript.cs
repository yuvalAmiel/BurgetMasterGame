using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrillScript : MonoBehaviour
{

    public bool rightGrillCollision = false;
    public GameObject myPlayer;
    public joyButton m_getInfo;
    leftSideCollision m_LeftBurger, m_RightBurger;
    public int maxBurgers = 2;
    Animator playerAnimator;
    public bool youCanClick = true;
    Animator TimeUpAnimatorLeft, TimeUpAnimatorRight;
    bool LeftBurgerOn = false, RightBurgerOn = false;
    Transform RightBurger = null, LeftBurger = null;
    burgerStates RightBurgerState, LeftBurgerState;
    void Start()
    {
        m_LeftBurger = this.transform.GetChild(0).GetComponent<leftSideCollision>();
        m_RightBurger = this.transform.GetChild(4).GetComponent<leftSideCollision>();

        playerAnimator = myPlayer.GetComponent<Animator>();
        TimeUpAnimatorRight = this.transform.GetChild(2).GetComponent<Animator>();
        TimeUpAnimatorLeft = this.transform.GetChild(3).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimators();            

        if (!youCanClick)
        {
            if (m_getInfo.ReturnIfPressed())
                return;
            youCanClick = true;
        }


        if ((m_RightBurger.CollideWithPlayer || m_LeftBurger.CollideWithPlayer) && m_getInfo.ReturnIfPressed() &&
            !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Running")
             && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("RunningWith")) // if theres a collision and the pickup button is pressed
        {
            youCanClick = false;

            if (myPlayer.transform.childCount <= 2)
            {
                if (m_RightBurger.CollideWithPlayer && RightBurgerOn) // take out the burger on the right
                {
                    if(TakeOutBurger(RightBurger, RightBurgerState, TimeUpAnimatorRight))
                    {
                        RightBurger = null;
                        RightBurgerState = null;
                        RightBurgerOn = false;
                    }
                }
                   
                else if (m_LeftBurger.CollideWithPlayer && LeftBurgerOn) // take out the burger on the left
                {
                    if(TakeOutBurger(LeftBurger, LeftBurgerState, TimeUpAnimatorLeft))
                    {
                        LeftBurger = null;
                        LeftBurgerState = null;
                        LeftBurgerOn = false;
                    }
                }

                if (!RightBurger && !LeftBurger)
                    FindObjectOfType<AudioManager>().Stop("BurgerOnGrill");

                return;
            }

            Transform myChild = myPlayer.transform.Find("Burger(Clone)").transform;
            if (myChild != null) // grill accepts only Burgers!!!
            {

                if (m_RightBurger.CollideWithPlayer && !RightBurgerOn) // place burger on the right
                {
                    RightBurger = myChild;
                    RightBurgerState = myChild.GetComponent<burgerStates>();
                    PlaceBurgerOnGrill(RightBurger, new Vector3(0.8f, 0.65f, 0.8f),
                        RightBurgerState, TimeUpAnimatorRight);
                    RightBurgerOn = true;
                    
                }
                else if (m_LeftBurger.CollideWithPlayer && !LeftBurgerOn) // place burger on the left
                {
                    LeftBurger = myChild;
                    LeftBurgerState = myChild.GetComponent<burgerStates>();
                    PlaceBurgerOnGrill(LeftBurger, new Vector3(-0.8f, 0.65f, 0.8f),
                        LeftBurgerState, TimeUpAnimatorLeft);
                    LeftBurgerOn = true;
                   
                }

                if (!FindObjectOfType<AudioManager>().IsPlaying("BurgerOnGrill"))
                    FindObjectOfType<AudioManager>().Play("BurgerOnGrill");
            }

        }
    }

    private void UpdateAnimators()
    {
        if (!RightBurgerOn && !LeftBurgerOn)
        {
            TimeUpAnimatorLeft.SetBool("Burned", false);
            TimeUpAnimatorLeft.SetBool("OldBurger", false);
            TimeUpAnimatorLeft.SetBool("TimeIsUp", false);

            TimeUpAnimatorRight.SetBool("Burned", false);
            TimeUpAnimatorRight.SetBool("OldBurger", false);
            TimeUpAnimatorRight.SetBool("TimeIsUp", false);
        }
        else
        {
            if (RightBurgerOn)
            {
                TimeUpAnimatorRight.SetBool("TimeIsUp", true);
                CheckAnimationState(RightBurgerState, TimeUpAnimatorRight);
            }
            if (LeftBurgerOn) 
            {
                TimeUpAnimatorLeft.SetBool("TimeIsUp", true);
                CheckAnimationState(LeftBurgerState, TimeUpAnimatorLeft);
            }
        }

    }

    public void CheckAnimationState(burgerStates BurgerState, Animator TimeUp)
    {
        if (TimeUp.GetCurrentAnimatorStateInfo(0).IsName("RareState"))
            BurgerState.UpdateBurgerState("Rare");
        else if (TimeUp.GetCurrentAnimatorStateInfo(0).IsName("MediumState"))
            BurgerState.UpdateBurgerState("Medium");
        else if (TimeUp.GetCurrentAnimatorStateInfo(0).IsName("ReadyState")
            || TimeUp.GetCurrentAnimatorStateInfo(0).IsName("ReadyStateContinue"))
            BurgerState.UpdateBurgerState("Ready");
        else if (TimeUp.GetCurrentAnimatorStateInfo(0).IsName("BurnedState"))
            BurgerState.UpdateBurgerState("Burned");
        else
            Debug.Log("not a single if");
    }


    private bool TakeOutBurger(Transform Burger, burgerStates BurgerState, Animator TimeUp)
    {
        if (!ValidateBurgerState(BurgerState))
            return false;

        Burger.transform.SetParent(myPlayer.transform);
        TimeUp.SetBool("Burned", false);
        TimeUp.SetBool("OldBurger", false);
        TimeUp.SetBool("TimeIsUp", false);
        return true;
    }

    private void PlaceBurgerOnGrill(Transform Burger, Vector3 localPosition, burgerStates BurgerState, Animator TimeUp)
    {
        Burger.transform.SetParent(this.transform);
        Burger.transform.localPosition = localPosition;
        
        if (ValidateBurgerState(BurgerState)) // making sure if the new burger is already burned/done
        {
            if (BurgerState.GetState() == "Burned")
                TimeUp.SetBool("Burned", true);
            else
                TimeUp.SetBool("OldBurger", true);
        }

        TimeUp.SetBool("TimeIsUp", true);
    }

    bool ValidateBurgerState(burgerStates BurgerState)
    {
        if (BurgerState != null)
            return (BurgerState.GetState() == "Ready" || BurgerState.GetState() == "Burned");
        return false;
    }
}
