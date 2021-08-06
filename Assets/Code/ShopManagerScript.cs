using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManagerScript : MonoBehaviour
{

   //public int[,] shopItems = new int[3, 5];
    private CoinCollection m_CoinCollection;

    void Start()
    {
        m_CoinCollection = FindObjectOfType<CoinCollection>();
        /*
        //ID's
        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;
        shopItems[1, 4] = 4;

        //Price
        shopItems[2, 1] = 10;
        shopItems[2, 2] = 20;
        shopItems[2, 3] = 30;
        shopItems[2, 4] = 40;

    */

    }


    public void Buy()
    {
        /*
        Debug.Log("in buyyyy");
        GameObject ButtonRef = 
            GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        int ItemValue = ButtonRef.GetComponent<ButtonInfo>().ItemValue;
        Debug.Log(m_CoinCollection.Coins);
        if (m_CoinCollection.Coins >= ItemValue)
        {
            m_CoinCollection.Coins -= ItemValue;
            ButtonRef.GetComponent<ButtonInfo>().m_Container.GetComponent<ChangeForIngridient>().ReplaceToIngridient();
            Destroy(ButtonRef);
        }
        */

    }
}
