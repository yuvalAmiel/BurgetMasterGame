using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : MonoBehaviour
{
    public GameObject myPlayer;
    public bool m_Collision = false, m_Pressed = false;
    public bool youCanClick = true, m_FoodOn = false;
    Animator PlayerAnimator, TimerAnimator;
    Transform m_BoardKnife;
    float tempTime = 0;
    Transform m_Food = null;
    bool changed = false;
    void Start()
    {
        PlayerAnimator = myPlayer.GetComponent<Animator>();
        m_BoardKnife = this.transform.GetChild(1);
        TimerAnimator = this.transform.GetChild(2).GetComponent<Animator>();
        if (this.GetComponentInParent<emptyContainer>().myFood != null)
            Destroy(this.GetComponentInParent<emptyContainer>().myFood.gameObject, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        m_Collision = this.GetComponentInParent<emptyContainer>().isCollideWithPlayer;
        m_Pressed = this.GetComponentInParent<emptyContainer>().m_getInfo.ReturnIfPressed();



        if (!youCanClick)
        {
            if (m_Pressed)
                return;
            youCanClick = true;
        }

        if (m_Pressed)
            youCanClick = false;

        if (!m_FoodOn && myPlayer.transform.childCount > 2 && m_Collision && m_Pressed)
        {
            if (myPlayer.transform.GetChild(2).tag == "Cuttable") 
            {
                changed = false;
                m_FoodOn = true;
                m_BoardKnife.gameObject.SetActive(false); 
                PlayerAnimator.SetBool("CuttingAnimation", true);
                TimerAnimator.SetBool("GoHere", true);
                TimerAnimator.SetBool("Start", true);
                FindObjectOfType<AudioManager>().Play("Cutting");
                tempTime = Time.realtimeSinceStartup;
                Transform cuttableObj = myPlayer.transform.GetChild(2);
                cuttableObj.SetParent(this.transform);
                cuttableObj.localPosition = new Vector3(0, 0.046f, 0);
                m_Food = cuttableObj;
            }
        }

        if (m_Food && !changed && Time.realtimeSinceStartup - tempTime > 3)
        {
            m_Food.GetComponent<ChangeToSliced>().ChangeObject();
            changed = true;
        }
        if(TimerAnimator.GetCurrentAnimatorStateInfo(0).IsName("MiddleState"))
        {

            FindObjectOfType<AudioManager>().Stop("Cutting");
            TimerAnimator.SetBool("GoHere", false);
            TimerAnimator.SetBool("Start", false);
            PlayerAnimator.SetBool("CuttingAnimation", false);
            m_Food.tag = "Sliced";
            m_Food.SetParent(myPlayer.transform);
            m_BoardKnife.gameObject.SetActive(true);
            m_FoodOn = false;
        }
    }
}
