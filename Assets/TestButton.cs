using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    [SerializeField]
    ProgressBox progressBox;

    public void TestClick()
    {
        progressBox.UpdateProgress(0.1f);
        Debug.Log("in click");
    }
}
