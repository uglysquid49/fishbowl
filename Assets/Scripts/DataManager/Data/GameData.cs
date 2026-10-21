using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]

public class GameData
{
    public Vector3 playerPosition;

     public GameData()
    {
        playerPosition = Vector3.zero;
        //game starts with no data to load
        Debug.Log("saving");
    }
   
}
