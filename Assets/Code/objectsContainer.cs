using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectsContainer : MonoBehaviour
{
    public bool m_isCollideWithPlayer = false;
    public Transform myFood;
    public GameObject myPlayer;
    public joyButton m_getInfo;
    Animator m_Animator;
    TutorialManager m_Tutorial;
    public bool youCanClick = true;
    PlayerCollisions m_UpdateCollision;
    public bool ToBeContainer = false;
    Animator playerAnimator;
    public int m_TutorialIndex = 0;
    void Start()
    {
        if (!ToBeContainer) // if it's a container from start - get Animator
            m_Animator = GetComponent<Animator>();
        else
            m_Animator = this.GetComponentInChildren<Animator>(); // if not - get Animator of Child
        m_UpdateCollision = myPlayer.GetComponent<PlayerCollisions>(); // only 1 container opens at a time
        playerAnimator = myPlayer.GetComponent<Animator>();
        m_Tutorial = FindObjectOfType<TutorialManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Tutorial.InTutorial() && m_Tutorial.GetCurrIndex() != m_TutorialIndex)
            return;
        if (!youCanClick)
        {
            if (m_getInfo.ReturnIfPressed())
                return;
            youCanClick = true;
        }

        if (m_isCollideWithPlayer && m_getInfo.ReturnIfPressed() &&
            !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Running")
             && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("RunningWith")) // if theres a collision and the pickup button is pressed
        {
            youCanClick = false;
            if (myPlayer.transform.childCount <= 2) // doesn't hold a thing
            {
                Debug.Log("give food");
                Instantiate(myFood).transform.SetParent(myPlayer.transform);
            }
        }
    }


    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && !m_UpdateCollision.getInCollision())
        {
            m_UpdateCollision.setInCollision(true);
            if (m_Animator != null)
                m_Animator.SetBool("IsCollideWithPlayer", true);
            else
                Debug.Log("sdfds");
            FindObjectOfType<AudioManager>().Play("OpeningBox");
            m_isCollideWithPlayer = true;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_UpdateCollision.setInCollision(false);
            if (m_Animator != null)
                m_Animator.SetBool("IsCollideWithPlayer", false);
            m_isCollideWithPlayer = false;

        }
    }
}
