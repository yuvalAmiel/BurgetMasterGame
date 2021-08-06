using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderHandler : MonoBehaviour
{
    bool m_State = false;
    float m_timeOfActive = 0;
    public float orderTime = 0;
    public int m_OrderValue = 10;
    public float m_ProgressValue = 0.5f;
    private Animator m_Animator;
    TutorialManager m_tempManager;
    
    void Awake()
    {
        m_Animator = this.transform.GetChild(2).GetComponent<Animator>();
        m_tempManager = FindObjectOfType<TutorialManager>();
    }

    public void ChangeTime(float time)
    {
        orderTime = time;
    }

    public void setIngradients()
    {
        FindObjectOfType<AudioManager>().Play("IconPress");
        m_State = !m_State;
        this.transform.GetChild(0).gameObject.SetActive(m_State);
        if (m_State)
        {
            m_timeOfActive = Time.realtimeSinceStartup;
            m_tempManager.OrderPressed = true;
        }
    }

    public void StartAlertAnimation()
    {
        m_Animator.SetBool("Warning", true);
    }

    public bool GetTimeAnimationState()
    {
        if (m_tempManager.InTutorial())
            return false;
        return m_Animator.GetCurrentAnimatorStateInfo(0).IsName("endTime");
    }

    public float getActiveTime()
    {
        if (!this.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            m_timeOfActive = 0;
            m_State = false;
        }
           
        return m_timeOfActive;
    }

    public int getOrderValue()
    {
        return m_OrderValue;
    }

    public float getProgressValue()
    {
        return m_ProgressValue;
    }

    public bool getState()
    {
        return m_State;
    }
}
