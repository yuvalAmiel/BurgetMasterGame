using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFoodPickUps : MonoBehaviour
{
    private bool m_InCollisionNow;
    // Start is called before the first frame update
    void Awake()
    {
        m_InCollisionNow = false;
    }

    public void setCollision(bool state)
    {
        m_InCollisionNow = state;
    }

    public bool getCollision()
    {
        return m_InCollisionNow;
    }
}
