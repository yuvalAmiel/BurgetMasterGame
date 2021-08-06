using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    public bool m_InCollision;
    void Awake()
    {
        m_InCollision = false;
    }

    public void setInCollision(bool state)
    {
        m_InCollision = state;
    }

    public bool getInCollision()
    {
        return m_InCollision;
    }
}
