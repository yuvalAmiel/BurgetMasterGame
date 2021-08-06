using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class burgerStates : MonoBehaviour
{
    public float m_BurgerTime = 0;
    public Animator m_Animator;
    Color m_Burned, m_Medium, m_Ready, m_Rare, tempColor;
    float updatedTime = 0;
    private string m_BurgerState;

    void Start()
    {
        //m_Animator = GetComponent<Animator>();
        m_Rare = this.gameObject.GetComponent<Renderer>().material.color;
        m_Medium = Color.HSVToRGB(0.023f, 0.647f, 0.747f);
        m_Ready = Color.HSVToRGB(0.044f, 0.707f, 0.567f);
        m_Burned = Color.HSVToRGB(0.004f, 1.000f, 0.000f);
        m_BurgerState = "Rare";
    }

    public string GetState()
    {
        return m_BurgerState;
    }

    private void updateColor(Color tempColor, string state)
    {
        this.gameObject.GetComponent<Renderer>().material.color = tempColor;
        if(state == "Ready" && m_BurgerState != state)
            FindObjectOfType<AudioManager>().Play("BurgerReady");
        m_BurgerState = state;
    }

    public void UpdateBurgerState(string state)
    {
        if (state == "Rare")
            tempColor = m_Rare;
        else if (state == "Medium")
            tempColor = m_Medium;
        else if (state == "Ready")
            tempColor = m_Ready;
        else
            tempColor = m_Burned;
        updateColor(tempColor, state);
        
    }
}
