using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewLevelCaller : MonoBehaviour
{

    ProgressBox ToUpdateLevel;
    // Start is called before the first frame update
    void Start()
    {
        ToUpdateLevel = FindObjectOfType<ProgressBox>();
    }

    public void CallMyUpdate()
    {
        ToUpdateLevel.UpdateMyLevel();
    }

}
