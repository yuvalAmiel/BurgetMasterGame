using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject m_TutorialPanel;
    public GameObject[] m_PopUps;

    public GameObject myPlayer;
    private int m_Index;
    PlateContainer m_PlateReference;
    private GameObject myPlate, myBun, myBurger;
    private bool m_InTutorial;
    public bool OrderPressed = false;
    
    void Start()
    {
        m_PlateReference = FindObjectOfType<PlateContainer>();
        m_InTutorial = true;
        m_TutorialPanel.SetActive(true);
    }

    public bool InTutorial()
    {
        return m_InTutorial;
    }

    public int GetCurrIndex()
    {
        return m_Index;
    }

    public void TurnOffPanel()
    {
        m_TutorialPanel.SetActive(false);
    }

    void Update()
    {
        Debug.Log("index is " + m_Index + " and length is " + m_PopUps.Length);
        if (m_TutorialPanel.activeInHierarchy && m_Index < m_PopUps.Length)
        {
            for (int i = 0; i < m_PopUps.Length; i++)
            {
                if (i == m_Index)
                {
                    m_PopUps[m_Index].SetActive(true);
                    Debug.Log("inex is " + i);
                }
                else
                    m_PopUps[i].SetActive(false);
            }

            if (m_Index == 0) // Starting - Take A Plate
            {
                if (m_PlateReference.getHowManyPlatesTaken() == 1)
                {
                    myPlate = myPlayer.transform.GetChild(2).gameObject;
                    m_Index++;
                }
            }
            else if (m_Index == 1) // Holds Plate - Place On Counter
            {
                if (myPlate.GetComponentInParent<emptyContainer>() != null) // Plate On Container
                    m_Index++;
            }
            else if (m_Index == 2) // now needs to take a bun
            {
                if (myPlayer.transform.childCount > 2)
                    if (myPlayer.transform.GetChild(2).name == "Bread(Clone)")
                    {
                        myBun = myPlayer.transform.GetChild(2).gameObject;
                        m_Index++;
                    }
            }
            else if (m_Index == 3) 
            {
                if (myBun.transform.parent.tag == "Plate")
                    m_Index++;
                OrderPressed = false;
            }
            else if(m_Index == 4) //bun is on plate, now check what's the ingridients
            {
                if (OrderPressed)
                    m_Index++;
            }
            else if (m_Index == 5)
            {
                if (myPlayer.transform.childCount > 2)
                {
                    if (myPlayer.transform.GetChild(2).name == "Burger(Clone)")
                    {
                        myBurger = myPlayer.transform.GetChild(2).gameObject;
                        m_Index++;
                    }
                }
            }
            else if (m_Index == 6)
            {
                if (myBurger.transform.parent.name == "GRILL")
                    m_Index++;
            }
            else if (m_Index == 7)
            {
                if (myBurger.transform.parent.tag == "Player")
                    m_Index++;
            }
            else if (m_Index == 8)
            {
                if (myBurger.transform.parent.name == "Bread(Clone)")
                    m_Index++;
            }
            else if(m_Index == 9)
            {
                if (myPlate.transform.parent.name == "Checker")
                    m_Index++;
            }
            Debug.Log(m_Index);
        }
        else
        {
            Debug.Log("in elseee");
            for (int i = 0; i < m_PopUps.Length; i++)
                    m_PopUps[i].SetActive(false);
            m_InTutorial = false;
            m_TutorialPanel.SetActive(false);
        }
    }
}
