using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
    AudioManager forWalking;

    public joyButton m_btn;

    protected Joystick joystick;
    Vector3 m_movement;
    Quaternion m_Rotation = Quaternion.identity;
    Rigidbody m_rigidbody;
    public bool m_sinkCollide = false;
    public float washDishes = 0;
    float tempAnimatorSpeed = 2.2f;
    DirtyPlate m_DirtyPlates;
    Animator m_Animator;
    public float turnSpeed = 20f;
    GameObject sink;

    Animator tempAnimator = null;
    ProgressBox animCondition;
    CoinCollection m_CoinCollection;
    GameObject knife;
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("ResturantNoise");
        knife = this.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1" +
            "/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm" +
            "/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandPinky1/Knife").gameObject;
        if (knife != null)
            Debug.Log("wow");
        else
            Debug.Log("sdfds");

        animCondition = FindObjectOfType<ProgressBox>();
        m_DirtyPlates = FindObjectOfType<DirtyPlate>();
        m_CoinCollection = FindObjectOfType<CoinCollection>();
        m_Animator = GetComponent<Animator>();
        joystick = FindObjectOfType<Joystick>();
        m_rigidbody = GetComponent<Rigidbody>();

        forWalking = FindObjectOfType<AudioManager>();
    }

    void FixedUpdate()
    {
        if (m_sinkCollide)
            handleSinkAction();

        if (m_Animator.GetBool("CuttingAnimation"))
        {
            knife.SetActive(true);
            return;
        }
        else
            knife.SetActive(false);

        if (animCondition.tempBoolForAnim )
            return;

        if (m_CoinCollection.inCoinMovement)
        {
            m_Animator.SetBool("IsHolding", false);
            return;
        }
        
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("pickUp"))
        {
            m_rigidbody.velocity = new Vector3(joystick.Vertical * 1,
                                                m_rigidbody.velocity.y,
                                                joystick.Horizontal * 1);
            Debug.Log("try to move so return");
            return;
        }

        /*
        if(this.transform.childCount > 2) // hold something
        {

        }*/

        if(!m_Animator.GetBool("timeToWash"))
            m_Animator.SetBool("isButtonClicked", m_btn.ReturnIfPressed());

        float horizontal = joystick.Horizontal * 10f;
        float vertical = joystick.Vertical * 10f;

        m_rigidbody.velocity = new Vector3(joystick.Vertical * -10,
                                                m_rigidbody.velocity.y,
                                                joystick.Horizontal * 10);

        
        bool hasHorizontalInput = !Mathf.Approximately(vertical, 0f);
        bool hasVerticalInput = !Mathf.Approximately(horizontal, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        
        
        for(int i = 0; i < this.transform.childCount; i++) // going threw all childs
        {
            Transform child = this.transform.GetChild(i);
            if(child.tag == "Food" || child.tag == "Plate" || child.tag == "Cuttable" || child.tag == "Sliced") // if theres a Food child, then player isHoling, and break
            {
                Debug.Log(child.tag + " " + i);
                m_Animator.SetBool("IsHolding", true);
                Vector3 playerLoc = this.transform.localPosition;

               

                if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("standingWith"))
                {
                    if(child.tag == "Food" || child.tag == "Cuttable" || child.tag == "Sliced")
                        child.localPosition = new Vector3(playerLoc.x + 10, playerLoc.y + 45, playerLoc.z + 30);
                    else
                        child.localPosition = new Vector3(playerLoc.x + 20, playerLoc.y + 60, playerLoc.z + 40);

                }

                break;
                //10, 45, 30
            }
            m_Animator.SetBool("IsHolding", false); // if never break, then 
        }
        if (isWalking && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("pickUp"))
            return; // maybe this change will work
        else
            m_Animator.SetBool("IsWalking", isWalking);


       


        if (isWalking)
        {
            if (!forWalking.IsPlaying("PlayerWalk"))
                forWalking.Play("PlayerWalk");
        }
        else
            forWalking.Stop("PlayerWalk");


        

        Vector3 desiredForward = Vector3.RotateTowards
            (transform.forward, m_rigidbody.velocity, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    public void handleSinkAction()
    {
        if (m_btn.ReturnIfPressed() && m_DirtyPlates.getState())
        {
            if (this.transform.childCount == 2) // doesn't hold a thing
            {
                if (sink != null)
                {
                    if (tempAnimator != null)
                        tempAnimator.speed = tempAnimatorSpeed;
                    else
                        tempAnimator = sink.transform.GetChild(2).GetComponent<Animator>();
                    tempAnimator.SetBool("Start", true);
                }
                m_Animator.SetBool("timeToWash", true);
                if (!FindObjectOfType<AudioManager>().IsPlaying("WashingDishes"))
                    FindObjectOfType<AudioManager>().Play("WashingDishes");
            }
        }


        //think about better condition here
        if(tempAnimator != null)
        {
            if(AnimatorIsPlaying("MiddleState"))
            {
                tempAnimator.SetBool("Start", false);
                tempAnimator.SetBool("GoHere", false);
                m_DirtyPlates.changePlatesActiveState(false);
                m_DirtyPlates.m_CleanPlates.ShowPlates();
                m_Animator.SetBool("timeToWash", false);
                FindObjectOfType<AudioManager>().Stop("WashingDishes");
            }
        }

    }

    bool AnimatorIsPlaying(string stateName)
    {
        return tempAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    public void OnAnimatorMove()
    {

        m_rigidbody.MovePosition
            (m_rigidbody.position + m_rigidbody.velocity * m_Animator.deltaPosition.magnitude);
        m_rigidbody.MoveRotation(m_Rotation);


    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Sink"))
        {
            m_sinkCollide = true;
            sink = other.gameObject;
        }

    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Sink"))
        {
            m_Animator.SetBool("timeToWash", false);
            FindObjectOfType<AudioManager>().Stop("WashingDishes");
            m_sinkCollide = false;
            if (tempAnimator != null && tempAnimator.GetCurrentAnimatorStateInfo(0).IsName("SinkTimer"))
            {
                //tempAnimatorSpeed = tempAnimator.speed;
                tempAnimator.speed = 0;
            }       
        }
    }

}
