using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isPlantable = true;

    public bool IsPlantable()
    {
        return isPlantable;
    }
}