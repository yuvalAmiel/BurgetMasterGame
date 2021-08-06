using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodLocation : MonoBehaviour
{
    public Vector3 foodPositionOnContainer;
    public Vector3 foodPositionOnPlate;

    public Vector3 getWantedPosition()
    {
        return foodPositionOnContainer;
    }
    public Vector3 getWantedPositionForPlate()
    {
        return foodPositionOnPlate;
    }
}
