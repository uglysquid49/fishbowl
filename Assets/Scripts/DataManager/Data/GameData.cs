using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

[System.Serializable]

public class GameData
{
    public Vector3 playerPosition;

    public bool feedingGameCompleted;
    public bool quizGameCompleted;
    public int quizHighScore;

    public GameData()
    {
        playerPosition = Vector3.zero; //default

        //game starts with no data to load
        this.feedingGameCompleted = false;
        this.quizGameCompleted = false;
        this.quizHighScore = 0;

        Debug.Log("saving");
    }
   
}
