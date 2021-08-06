using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leftSideCollision : MonoBehaviour
{
    public bool CollideWithPlayer = false;
    void OnCollisionEnter(Collision other)
    {
        CollideWithPlayer = true;
    }

    void OnCollisionExit(Collision other)
    {
        CollideWithPlayer = false;
    }
}
