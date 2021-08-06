using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeForIngridient : MonoBehaviour
{
    public GameObject myContainer;
    
    public void ReplaceToIngridient()
    {
        /*
        this.gameObject.GetComponent<emptyContainer>().enabled = false;

        Destroy(gameObject);
        myContainer.transform.localScale = new Vector3(1.1f, 1f, 1.12f);
        myContainer.transform.position = new Vector3(-5.4f, 0f, this.transform.position.z);
        Instantiate(myContainer, myContainer.transform.position, myContainer.transform.rotation);

        this.gameObject.GetComponent<objectsContainer>().enabled = true;*/

        myContainer.transform.localScale = new Vector3(1.1f, 1f, 1.12f);
        myContainer.transform.position = new Vector3(-5.4f, 0f, this.transform.position.z);
        this.gameObject.GetComponent<emptyContainer>().enabled = false;
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;

        Destroy(this.gameObject.GetComponent<emptyContainer>());

        Instantiate(myContainer, myContainer.transform.position, myContainer.transform.rotation).transform
            .SetParent(this.transform);
        this.gameObject.GetComponent<objectsContainer>().enabled = true;
    }
}
