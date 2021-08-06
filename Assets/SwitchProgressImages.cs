using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SwitchProgressImages : MonoBehaviour
{

    ProgressBox m_LevelReference;
    public Sprite[] m_GoalImages;
    Image m_ImageReference;
    int GiftCounter = 0;
    void Start()
    {
        m_LevelReference = FindObjectOfType<ProgressBox>();
        m_ImageReference = this.GetComponent<Image>();
        m_ImageReference.sprite = m_GoalImages[GiftCounter];
    }

    // Update is called once per frame
    void Update()
    {
        GiftCounter = m_LevelReference.level;
        if(!m_LevelReference.LevelUpMenu.gameObject.activeInHierarchy)
            m_ImageReference.sprite = m_GoalImages[GiftCounter];
    }
}
