using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeToSliced : MonoBehaviour
{
    public GameObject m_SlicedObject;

    void Start()
    {
        
    }

    public void ChangeObject()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        m_SlicedObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Instantiate(m_SlicedObject, this.transform.position, m_SlicedObject.transform.rotation).transform
           .SetParent(this.transform);
    }
}
